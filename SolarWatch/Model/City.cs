namespace SolarWatch.Model;

public class City
{
    public int CityId { get; set; }
    public string Name { get; init; }
    public int Longitude { get; init; }
    public int Latitude { get; init; }
    public string State { get; init; }
    public string Country { get; init; }
    
    public SetRiseTime SetRise { get; set; }
    
}