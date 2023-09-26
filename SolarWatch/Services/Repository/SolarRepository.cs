using SolarWatch.Model;

namespace SolarWatch.Repository;

public class SolarRepository : ISolarRepository
{
    private readonly IConfiguration _config;

    public SolarRepository(IConfiguration config)
    {
        _config = config;
    }
    
    
    public IEnumerable<City> GetAll()
    {
        using var dbContext = new SolarWatchApiContext(_config);
        return dbContext.Cities.ToList();
    }

    public City? GetByName(string name)
    {
        using var dbContext = new SolarWatchApiContext(_config);
        return dbContext.Cities.FirstOrDefault(x => x.Name == name);
    }

    public void Add(City city)
    {
        using var dbContext = new SolarWatchApiContext(_config);
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        using var dbContext = new SolarWatchApiContext(_config);
        dbContext.Remove(city);
        dbContext.SaveChanges();
    }

    public void Update(City city)
    {
        using var dbContext = new SolarWatchApiContext(_config);
        dbContext.Update(city);
        dbContext.SaveChanges();
    }
}