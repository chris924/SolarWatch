namespace SolarWatch;

public interface IJsonProcessor
{
    SunriseSunsetModel ProcessSunriseSunsetJson(string data, string city);
    LatLonModel ProcessCityJson(string data);
}