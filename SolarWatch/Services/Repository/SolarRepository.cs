using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;
using SolarWatch.Repository;

namespace SolarWatch.Services.Repository;

public class SolarRepository : ISolarRepository
{

    private SolarWatchApiContext dbContext;

  
    
    
    public SolarRepository(IConfiguration config)
    {
        dbContext = new SolarWatchApiContext(new DbContextOptions<SolarWatchApiContext>());
    }
    
    
    public IEnumerable<City> GetAll()
    {
        return dbContext.Cities.ToList();
    }

    public City? GetByName(string name)
    {
        var cityResult = dbContext.Cities.FirstOrDefault(x => x.Name == name);
        if (cityResult != null)
        {
            cityResult.SetRise = GetSetRiseByCity(cityResult);

            return cityResult;
        }

        return null;
    }


    public SetRiseTime? GetSetRiseByCity(City city)
    {
        return dbContext.SetRiseTimes.FirstOrDefault(x => x.CityId == city.CityId);
    }
    
    

    public void Add(City city)
    {
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        dbContext.Remove(city);
        dbContext.SaveChanges();
    }

    public void Update(City city)
    {
        dbContext.Update(city);
        dbContext.SaveChanges();
    }
}