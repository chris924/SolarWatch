using System.Net;
using System.Reflection;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;
using SolarWatch;
using SolarWatch.Controllers;
using ILogger = Castle.Core.Logging.ILogger;
using NUnit.Framework;
using SolarWatch.Model;
using SolarWatch.Repository;
using SolarWatch.Services.Repository;

namespace TestProject1;

public class Tests
{

    [TestFixture]
    public class ApiAndJsonProcessTests
    {
        private Mock<ILogger<SunriseSunsetController>> _loggerMock;
        private Mock<ISunriseSunsetAPI> _sunriseSunsetApiMock;
        private Mock<IJsonProcessor> _jsonProcessorMock;
        private Mock<IGeoLocatingAPI> _geoLocatingApiMock;
        private Mock<IConfiguration> _configMock;
        private Mock<ISolarRepository> _solarRepositoryMock;
        private Mock<DbContext> _solarWatchApiContextMock;
        private ILogger<JsonProcessor> _logger1;
        private ILogger<GeoLocatingAPI> _logger2;
        private ILogger<SunriseSunsetAPI> _logger3;
        private SunriseSunsetController _controller;
        private IJsonProcessor _jsonProcessor;
        private IGeoLocatingAPI _geoLocatingApi;
        private ISunriseSunsetAPI _sunriseSunsetApi;
        


        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<SunriseSunsetController>>();
            _sunriseSunsetApiMock = new Mock<ISunriseSunsetAPI>();
            _jsonProcessorMock = new Mock<IJsonProcessor>();
            _geoLocatingApiMock = new Mock<IGeoLocatingAPI>();
            _configMock = new Mock<IConfiguration>();
            _solarRepositoryMock = new Mock<ISolarRepository>();
            _solarWatchApiContextMock = new Mock<DbContext>();
            _logger1 = new Logger<JsonProcessor>(new LoggerFactory());
            _logger2 = new Logger<GeoLocatingAPI>(new LoggerFactory());
            _logger3 = new Logger<SunriseSunsetAPI>(new LoggerFactory());
            _controller = new SunriseSunsetController(_loggerMock.Object, _sunriseSunsetApiMock.Object,
                _jsonProcessorMock.Object, _geoLocatingApiMock.Object, _solarRepositoryMock.Object);
            _jsonProcessor = new JsonProcessor(_logger1);
            _geoLocatingApi = new GeoLocatingAPI(_logger2);
            _sunriseSunsetApi = new SunriseSunsetAPI(_logger3);

        }
        
        [Test]
        public async Task GetCurrent_ReturnsNotFound_IfCityNameIsInvalid()
        {
            var cityName = ","; 
            
            _geoLocatingApiMock.Setup(api => api.GetLatLon(cityName)).Throws(new Exception("Invalid city"));

            
            var result = await _controller.Get(cityName);
            
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
        
        [Test]
        public async Task GeoLocationApi_TypeIsValid_IfDataIsValid()
        {
            var expectedType = "string";
            
            _geoLocatingApiMock.Setup(x => x.GetLatLon(It.IsAny<string>())).ReturnsAsync(expectedType);

            var result =  await _geoLocatingApi.GetLatLon("London");
            
            Assert.IsInstanceOf(expectedType.GetType(), result);
        }

        [Test]
        public async Task JsonCityProcessor_IsValid_IfDataIsValid()
        {
            
            var cityName = "London";
            var expectedLatLonModel = new LatLonModel
            {
                City = "London",
                Lat = 51.507321900000001d,
                Lon = -0.12764739999999999d
            };

           
            _jsonProcessorMock.Setup(x => x.ProcessCityJson(It.IsAny<string>()))
                .Returns(expectedLatLonModel);

            var cityData = await _geoLocatingApi.GetLatLon(cityName);

           
            var result = _jsonProcessor.ProcessCityJson(cityData);
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf(typeof(LatLonModel)));
                Assert.That(result.City, Is.EqualTo(expectedLatLonModel.City));
                Assert.That(result.Lat, Is.EqualTo(expectedLatLonModel.Lat));
                Assert.That(result.Lon, Is.EqualTo(expectedLatLonModel.Lon));
            });
        }

        [Test]
        public async Task SunriseSunsetApiGetCurrentType_IsValid_IfDataIsValid()
        {
            var expectedType = "string";

            _sunriseSunsetApiMock.Setup(x => x.GetCurrent(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(expectedType);

            var result = await _sunriseSunsetApi.GetCurrent(51.507321900000001d, -0.12764739999999999d);
            
            Assert.That(result, Is.InstanceOf(expectedType.GetType()));
        }

        [Test]
        public void SolarRepository_ReturnsNull_IfCityIsNotInDb()
        {
            
            City? expectedResult = null;

            MethodInfo methodInfo = typeof(SunriseSunsetController).GetMethod("CheckDbForCity", BindingFlags.NonPublic | BindingFlags.Instance);
    
           
            var result = methodInfo.Invoke(_controller, new object[] { "SampleCity" }) as Task<City?>;
            
            
            Assert.That(expectedResult, Is.EqualTo(result.Result));
        }

        
    }
}