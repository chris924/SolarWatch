using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Authentication;

public class IdentityUsersContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{

    private readonly IConfiguration _config;
    
    public IdentityUsersContext(DbContextOptions<IdentityUsersContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = _config["SolarWatch:ConnectionString"];
        options.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
