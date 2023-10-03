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

    [HttpGet("get-sunrise-sunset"), Authorize(Roles = "User, Admin")]
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

            var newCity = CityCreator(latLonForCity.City, riseSetModel.Sunrise, riseSetModel.Sunset, latLonForCity.Lat,
                latLonForCity.Lon, latLonForCity.State, latLonForCity.Country);
            
            

           AddCityToDb(newCity);
            
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
        return  _solarRepository.GetByName(name);
    }


    private void AddCityToDb(City city)
    {
        _solarRepository.Add(city);
    }

    private City CityCreator(string city, TimeSpan sunrise, TimeSpan sunset, double lat, double lon, string state, string country)
    {
        SetRiseTime newSetRiseTime = new SetRiseTime
        {
            Sunrise = sunrise,
            Sunset = sunset
        };


        City newCity = new City
        {
            Name = city,
            Latitude = lat,
            Longitude = lon,
            Country = country,
            State = state,
            SetRise = newSetRiseTime 
        };


        newSetRiseTime.CityData = newCity;

        return newCity;

    }
    
    
    

    [HttpPost("UploadACityWithSunriseSunset"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<City>> UploadACityWithSunriseSunset(string city, TimeSpan sunrise, TimeSpan sunset, double lat, double lon, string state, string country)
    {
        try
        {
            
          var newCity = CityCreator(city, sunrise, sunset, lat, lon, state, country);
            
          _solarRepository.Add(newCity);

            return Ok(newCity);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error uploading data");
            return NotFound("Error uploading data");
        }
        
    }

    [HttpPatch("UpdateCityWithSunriseSunset"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<City>> UpdateACityWithSunriseSunset(string city, TimeSpan sunrise, TimeSpan sunset,
        double lat, double lon, string state, string country)
    {
        try
        {
            var updatedCity = _solarRepository.GetByName(city);
            
            if (updatedCity == null)
            {
                return BadRequest("Bad City name! Not found in database");
            }

            updatedCity.Country = country;
            updatedCity.Name = city;
            updatedCity.State = state;
            updatedCity.Latitude = lat;
            updatedCity.Longitude = lon;
            updatedCity.SetRise.Sunrise = sunrise;
            updatedCity.SetRise.Sunset = sunset;
            
           _solarRepository.Update(updatedCity);

            return Ok(updatedCity);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error modifying data");
            return NotFound("Error modifying data");
        }
    }
    
    
}