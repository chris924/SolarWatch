using Microsoft.AspNetCore.Mvc.Testing;

namespace SolarWatch.IntegrationTests.ControllerTests;

public class SunriseSunsetControllerTests : IDisposable
{
    private CustomWebApplicationFactory _factory;
    private HttpClient _client;

    private SunriseSunsetControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}