using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;
using SolarWatch.Repository;

namespace SolarWatch.Services.Repository;

public class SolarRepository : ISolarRepository
{

    private SolarWatchApiContext _dbContext;

  
    
    
    public SolarRepository(SolarWatchApiContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public IEnumerable<City> GetAll()
    {
        return _dbContext.Cities.ToList();
    }

    public City? GetByName(string name)
    {
        var cityResult = _dbContext.Cities.FirstOrDefault(x => x.Name == name);
        if (cityResult != null)
        {
            cityResult.SetRise = GetSetRiseByCity(cityResult);

            return cityResult;
        }

        return null;
    }


    public SetRiseTime? GetSetRiseByCity(City city)
    {
        return _dbContext.SetRiseTimes.FirstOrDefault(x => x.CityId == city.CityId);
    }
    
    

    public void Add(City city)
    {
        _dbContext.Add(city);
        _dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        _dbContext.Remove(city);
        _dbContext.SaveChanges();
    }

    public void Update(City city)
    {
        _dbContext.Update(city);
        _dbContext.SaveChanges();
    }
}