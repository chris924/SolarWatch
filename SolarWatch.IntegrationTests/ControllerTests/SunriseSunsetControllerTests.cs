using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace SolarWatch.IntegrationTests.ControllerTests
{
    [TestFixture]
    public class SunriseSunsetControllerTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
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
        public async Task Unauthorized_Get_Gives_Unauthorized()
        {
            var response = await _client.GetAsync("/SunriseSunset/get-sunrise-sunset?city=London");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }
    }
}