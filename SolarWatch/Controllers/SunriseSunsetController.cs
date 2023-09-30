using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunriseSunsetController : ControllerBase
{

    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly ISunriseSunsetAPI _sunriseSunsetApi;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly IGeoLocatingAPI _geoLocatingApi;
    private readonly ISolarRepository _solarRepository;
    

    public SunriseSunsetController(ILogger<SunriseSunsetController> logger, ISunriseSunsetAPI sunriseSunsetApi, IJsonProcessor jsonProcessor, IGeoLocatingAPI geoLocatingApi, ISolarRepository solarRepository)
    {
        _logger = logger;
        _sunriseSunsetApi = sunriseSunsetApi;
        _jsonProcessor = jsonProcessor;
        _geoLocatingApi = geoLocatingApi;
        _solarRepository = solarRepository;
    }

    [HttpGet("get-sunrise-sunset"), Authorize]
    public async Task<ActionResult<SunriseSunsetModel>> Get(string city)
    {

        var foundCityInDb =  await CheckDbForCity(city); // Check if City is already in database

        if (foundCityInDb != null) // If City in database, get the details from there
        {
            SunriseSunsetModel newRiseSetModel = new SunriseSunsetModel(){City = foundCityInDb.Name,
                Sunrise = foundCityInDb.SetRise.Sunrise, Sunset = foundCityInDb.SetRise.Sunset};
      
            return Ok(newRiseSetModel);
        }
        
        try 
        {
            
            LatLonModel latLonForCity = await GetLatLonForCity(city); // Latitude and Longitude values for city

            if (latLonForCity == null)
            {
                return StatusCode(500, "Error getting one or more city data.");
            }
            
            var riseSetModel = await GetSunriseSunsetData(latLonForCity.Lat, latLonForCity.Lon, latLonForCity.City); // Sunrise, sunset values with city name

           AddCityToDb(latLonForCity, riseSetModel);
            
            return Ok(riseSetModel);
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

    private async Task<SunriseSunsetModel> GetSunriseSunsetData(double lat, double lon, string city)
    {
        var riseSetData = await _sunriseSunsetApi.GetCurrent(lat, lon);
        return _jsonProcessor.ProcessSunriseSunsetJson( riseSetData, city);
    }

    internal async Task<City?> CheckDbForCity(string name)
    {
        return _solarRepository.GetByName(name);
    }


    private void AddCityToDb(LatLonModel model, SunriseSunsetModel result)
    {
        SetRiseTime newSetRiseTime = new SetRiseTime
        {
            Sunrise = result.Sunrise,
            Sunset = result.Sunset
        };


        City newCity = new City
        {
            Name = model.City,
            Latitude = model.Lat,
            Longitude = model.Lon,
            Country = model.Country,
            State = model.State,
            SetRise = newSetRiseTime 
        };


        newSetRiseTime.CityData = newCity;
            
        _solarRepository.Add(newCity);
    }
    
}