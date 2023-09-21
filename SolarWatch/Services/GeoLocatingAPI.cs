using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SolarWatch;

public class GeoLocatingAPI : IGeoLocatingAPI
{
    private readonly ILogger<GeoLocatingAPI> _logger;

    public GeoLocatingAPI(ILogger<GeoLocatingAPI> logger)
    {
        _logger = logger;
    }

    
    public async Task<string> GetLatLon(string city)
    {

        var apiKey = "981dcb85e6a6db2a60f9f2100efd7966";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apiKey}";
        
        _logger.LogInformation("Calling OpenWeather GeoLocating API with url: {url}", url);

        var client = new HttpClient();

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();

    }

    
}