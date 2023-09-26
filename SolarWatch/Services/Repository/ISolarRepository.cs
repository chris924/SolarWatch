using SolarWatch.Model;

namespace SolarWatch.Repository;

public interface ISolarRepository
{
    IEnumerable<City> GetAll();
    City? GetByName(string name);

    void Add(City city);
    void Delete(City city);
    void Update(City city);
}