namespace SolarWatch.Model;

public class SetRiseTime
{
    public int SetRiseTimeId { get; set; } // PRIMARY KEY
    public TimeSpan Sunrise { get; set; }
    public TimeSpan Sunset { get; set; }

    public int CityId { get; set; } // FOREIGN  KEY
    public City CityData { get; set; } // Navigation property!
}