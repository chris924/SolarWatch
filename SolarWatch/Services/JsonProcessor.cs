using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

namespace SolarWatch;

public class JsonProcessor : IJsonProcessor
{

    private readonly ILogger<JsonProcessor> _logger;

    public JsonProcessor(ILogger<JsonProcessor> logger)
    {
        _logger = logger;
    }


    public SunriseSunsetModel ProcessSunriseSunsetJson(string data, string city)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement main = json.RootElement.GetProperty("results");

        string sunriseString = main.GetProperty("sunrise").GetString();
        string sunsetString = main.GetProperty("sunset").GetString();
        
        DateTime sunriseTime = DateTime.ParseExact(sunriseString, "h:mm:ss tt", CultureInfo.InvariantCulture);
        DateTime sunsetTime = DateTime.ParseExact(sunsetString, "h:mm:ss tt", CultureInfo.InvariantCulture);
        
        SunriseSunsetModel forecast = new SunriseSunsetModel
        {
            City = city,
            Sunrise =  sunriseTime.TimeOfDay,
            Sunset =  sunsetTime.TimeOfDay
        };
        return forecast;
    }


    public LatLonModel ProcessCityJson(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement jsonArray = json.RootElement;
        
        JsonElement firstItem = jsonArray.EnumerateArray().FirstOrDefault();

        string name = firstItem.GetProperty("name").GetString();
        string country = firstItem.GetProperty("country").GetString();
        string state = firstItem.GetProperty("state").GetString();
        double lat = firstItem.GetProperty("lat").GetDouble();
        double lon = firstItem.GetProperty("lon").GetDouble();
            

            LatLonModel latLonModel = new LatLonModel
            {
                City = name,
                State = state,
                Country = country,
                Lat = lat,
                Lon = lon
            };
            return latLonModel;
            
        
    }
}