using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch;

public class SolarWatchApiContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SetRiseTime> SetRiseTimes { get; set; }
    
    private readonly IConfiguration _config;

    public SolarWatchApiContext(IConfiguration config)
    {
        _config = config;
    }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _config["SolarWatch:ConnectionString"];
        optionsBuilder.UseSqlServer(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<City>()
            .HasOne(city => city.SetRise)
            .WithOne(SetRise => SetRise.CityData)
            .HasForeignKey<SetRiseTime>(SetRise => SetRise.CityId);
    }
}