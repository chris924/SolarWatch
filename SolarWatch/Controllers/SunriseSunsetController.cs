using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunriseSunsetController : ControllerBase
{

    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly ISunriseSunsetAPI _sunriseSunsetApi;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly IGeoLocatingAPI _geoLocatingApi;
    
    

    public SunriseSunsetController(ILogger<SunriseSunsetController> logger, ISunriseSunsetAPI sunriseSunsetApi, IJsonProcessor jsonProcessor, IGeoLocatingAPI geoLocatingApi)
    {
        _logger = logger;
        _sunriseSunsetApi = sunriseSunsetApi;
        _jsonProcessor = jsonProcessor;
        _geoLocatingApi = geoLocatingApi;
    }

    [HttpGet("getsunrisesunset")]
    public async Task<ActionResult<SunriseSunsetModel>> Get(string city)
    {
        try
        {
            
            LatLonModel model = await GetLatLonForCity(city);
            
            
            var riseSetData = GetSunriseSunsetData(model.Lat, model.Lon);

            
            return Ok(_jsonProcessor.ProcessSunriseSunsetJson(await riseSetData, model.City));
        }
        catch (System.Net.WebException ex)
        {
            
            _logger.LogError(ex, "Error getting weather data from the API (Bad Request).");
            return StatusCode(500, "An error occurred while processing the request.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting weather data");
            return NotFound("Error getting weather data");
        }
    }

    private async Task<LatLonModel> GetLatLonForCity(string city)
    {
        var cityData = await _geoLocatingApi.GetLatLon(city);
        return _jsonProcessor.ProcessCityJson(cityData);
    }

    private async Task<string> GetSunriseSunsetData(double lat, double lon)
    {
        return await _sunriseSunsetApi.GetCurrent(lat, lon);
    }
}