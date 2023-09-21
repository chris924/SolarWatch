using System.Net;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;
using SolarWatch;
using SolarWatch.Controllers;
using ILogger = Castle.Core.Logging.ILogger;

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
            _logger1 = new Logger<JsonProcessor>(new LoggerFactory());
            _logger2 = new Logger<GeoLocatingAPI>(new LoggerFactory());
            _logger3 = new Logger<SunriseSunsetAPI>(new LoggerFactory());
            _controller = new SunriseSunsetController(_loggerMock.Object, _sunriseSunsetApiMock.Object,
                _jsonProcessorMock.Object, _geoLocatingApiMock.Object);
            _jsonProcessor = new JsonProcessor(_logger1);
            _geoLocatingApi = new GeoLocatingAPI(_logger2);
            _sunriseSunsetApi = new SunriseSunsetAPI(_logger3);

        }
        
        [Test]
        public async Task GetCurrentReturnsNotFoundIfCityNameIsInvalid()
        {
            var cityName = ",";

            var result =  await _controller.Get(cityName);
            
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }
        
        [Test]
        public async Task GeoLocationApiGetLatLonTypeIsValidIfDataIsValid()
        {
            var expectedType = "string";
            _geoLocatingApiMock.Setup(x => x.GetLatLon(It.IsAny<string>())).ReturnsAsync(expectedType);

            var result =  await _geoLocatingApi.GetLatLon("London");
            
            Assert.IsInstanceOf(expectedType.GetType(), result);
        }

        [Test]
        public async Task JsonCityProcessorIsValidIfDataIsValid()
        {
            // Arrange
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
        public async Task SunriseSunsetApiGetCurrentTypeIsValidIfDataIsValid()
        {
            var expectedType = "string";

            _sunriseSunsetApiMock.Setup(x => x.GetCurrent(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(expectedType);

            var result = await _sunriseSunsetApi.GetCurrent(51.507321900000001d, -0.12764739999999999d);
            
            Assert.That(result, Is.InstanceOf(expectedType.GetType()));
        }

       

        
    }
}