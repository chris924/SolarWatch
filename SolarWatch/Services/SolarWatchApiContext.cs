using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SolarWatch.Model;
using DotNetEnv;

namespace SolarWatch;

public class SolarWatchApiContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SetRiseTime> SetRiseTimes { get; set; }

    
    
    public SolarWatchApiContext() 
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTIONSTRING");
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<City>()
            .HasOne(city => city.SetRise)
            .WithOne(SetRise => SetRise.CityData)
            .HasForeignKey<SetRiseTime>(SetRise => SetRise.CityId);
    }
}