using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SolarWatch.Authentication;
using SolarWatch.Controllers;
using SolarWatch.Repository;
using SolarWatch.Services.Repository;

namespace SolarWatch.IntegrationTests.ControllerTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<ISolarRepository> SolarRepositoryMock { get; }
   public ISolarRepository SolarRepository { get; }
    public TestDbContext TestDbContext { get; }
    

    public CustomWebApplicationFactory()
    {
        SolarRepositoryMock = new Mock<ISolarRepository>();
        TestDbContext = new TestDbContext();
        SolarRepository = new SolarRepository(TestDbContext);

    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        
        

       builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(SolarRepositoryMock.Object);

            services.AddSingleton(SolarRepository);
           
            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

            services.AddDbContext<TestDbContext>();

        });

       builder.UseEnvironment("Test");
    }
    
    
}