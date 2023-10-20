using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace SolarWatch.IntegrationTests.ControllerTests;

public class TestDbContext : SolarWatchApiContext
{
    public TestDbContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "TestDb");
    }
}