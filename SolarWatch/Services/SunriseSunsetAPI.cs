using System.Net;

namespace SolarWatch;

public class SunriseSunsetAPI : ISunriseSunsetAPI
{
    private readonly ILogger<SunriseSunsetAPI> _logger;


    public SunriseSunsetAPI(ILogger<SunriseSunsetAPI> logger)
    {
        _logger = logger;
    }



    public async Task<string> GetCurrent(double lat, double lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date=today";

        var client = new HttpClient();
        
        _logger.LogInformation("Calling Sunrise-Sunset API with url: {url}", url);

        var response = await client.GetAsync(url);

        return await response.Content.ReadAsStringAsync();

    }
}                            