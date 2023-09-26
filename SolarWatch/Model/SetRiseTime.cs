namespace SolarWatch.Model;

public class SetRiseTime
{
    public int SetRiseTimeId { get; set; }
    public TimeSpan Sunrise { get; set; }
    public TimeSpan Sunset { get; set; }

    public int CityId { get; set; }
    public City CityData { get; set; }
}