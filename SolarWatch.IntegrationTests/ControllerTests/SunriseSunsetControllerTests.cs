using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SolarWatch.Controllers;
using SolarWatch.Model;
using SolarWatch.Repository;
using SolarWatch.Services.Repository;

namespace SolarWatch.IntegrationTests.ControllerTests
{
    [TestFixture]
    public class SunriseSunsetControllerTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        
        private Mock<ISolarRepository> _solarRepositoryMock;
        private Mock<TestDbContext> _testDbContextMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _solarRepositoryMock = new Mock<ISolarRepository>();
            _testDbContextMock = new Mock<TestDbContext>();
            
            _factory = new CustomWebApplicationFactory();

            _client = _factory.CreateClient();

          
            

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }


        [Test]
        public async Task Get_ChecksIfCityInDb()
        {


            var newSetRiseTime = new SetRiseTime
            {
                Sunrise = new TimeSpan(0),
                Sunset = new TimeSpan(0)

            };

            var mockCity = new City
            {
                Name = "mockCity",
                Latitude = 0,
                Longitude = 0,
                Country = "country",
                State = "state",
                SetRise = newSetRiseTime

            };

            newSetRiseTime.CityData = mockCity;
            
            
           _factory.SolarRepository.Add(mockCity);

           _factory.TestDbContext.SaveChanges();
           
            var response = await _client.GetAsync("SunriseSunset/get-sunrise-sunset?city=mockCity");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var cityFromDb = _factory.SolarRepository.GetByName("mockCity");
            Assert.That(cityFromDb, Is.EqualTo(mockCity));

        }


        [Test]
        public async Task Get_UploadsCityToDb_IfNotPresent()
        {
            var beforeUpload = _factory.SolarRepository.GetByName("London");
            
            var response = await _client.GetAsync("SunriseSunset/get-sunrise-sunset?city=London");

           var afterUpload = _factory.SolarRepository.GetByName("London");
            
           Assert.Multiple(() => 
           {
               Assert.That(beforeUpload, Is.EqualTo(null));
               Assert.That(afterUpload.Name, Is.EqualTo("London"));
           });
            
        }
       
    }
}