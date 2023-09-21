namespace SolarWatch;

public interface IGeoLocatingAPI
{
    Task<string> GetLatLon(string city);
}