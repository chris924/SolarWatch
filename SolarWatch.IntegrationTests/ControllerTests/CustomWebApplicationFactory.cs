using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SolarWatch.Authentication;
using SolarWatch.Repository;

namespace SolarWatch.IntegrationTests.ControllerTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<ISolarRepository> SolarRepositoryMock { get; }

    public CustomWebApplicationFactory()
    {
        SolarRepositoryMock = new Mock<ISolarRepository>();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(SolarRepositoryMock.Object);
            services.AddDbContext<SolarWatchApiContext>(options =>
            {
                options.UseInMemoryDatabase("InMemorySolarWatchDatabase"); 
            });
            services.AddDbContext<IdentityUsersContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryIdentity"); 
            });
        });

        builder.UseEnvironment("Test");
    }
}