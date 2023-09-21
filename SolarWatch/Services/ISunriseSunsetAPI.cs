namespace SolarWatch;

public interface ISunriseSunsetAPI
{
    Task<string> GetCurrent(double lat, double lon);
}