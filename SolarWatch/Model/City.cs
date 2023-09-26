namespace SolarWatch.Model;

public class City
{
    public int CityId { get; set; } // PRIMARY KEY
    public string Name { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    
    public SetRiseTime SetRise { get; set; } // Navigation property!
    
}