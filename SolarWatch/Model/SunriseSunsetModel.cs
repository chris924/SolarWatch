using SolarWatch.Model;

namespace SolarWatch;

public class SunriseSunsetModel
{
    public string City { get; set; }
    public TimeSpan Sunrise { get; set; }
    public TimeSpan Sunset { get; set; }
    
}