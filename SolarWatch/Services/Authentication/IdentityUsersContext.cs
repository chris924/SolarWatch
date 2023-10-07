using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Authentication;

public class IdentityUsersContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{

    
    
    public IdentityUsersContext(DbContextOptions<IdentityUsersContext> options) : base(options)
    {
    }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
