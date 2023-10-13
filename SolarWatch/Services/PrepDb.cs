using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Authentication;
using SolarWatch.Model;
using SolarWatch.Services.Repository;

namespace SolarWatch;

public static class PrepDb
{


    public static void PrepPopulation(IApplicationBuilder app, IWebHostEnvironment env)
    {

        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            
            if (!env.IsEnvironment("Test"))
            {
                Console.WriteLine($"Environment is: {env.EnvironmentName}, running PrepDb... ");
                SeedData(serviceScope.ServiceProvider.GetService<SolarWatchApiContext>(),
                    serviceScope.ServiceProvider.GetService<IdentityUsersContext>());
                AddRoles(serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
                AddAdmin(serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>());
            }
            Console.WriteLine($"Environment is: {env.EnvironmentName},not running PrepDb... ");
           
        }

        static void SeedData(SolarWatchApiContext solarContext, IdentityUsersContext identityContext)
        {
            System.Console.WriteLine("Applying Migrations....");

            Thread.Sleep(10000);

            Console.WriteLine(solarContext);
            
            solarContext.Database.Migrate();
            identityContext.Database.Migrate();


            Thread.Sleep(5000);

            if (!solarContext.Cities.Any())
            {
                System.Console.WriteLine("Adding data - seeding...");

                SetRiseTime newSetRiseTime = new SetRiseTime
                {
                    SetRiseTimeId = 0,
                    Sunrise = new TimeSpan(0),
                    Sunset = new TimeSpan(0)
                };


                City newCity = new City
                {
                    CityId = 0,
                    Name = "TEST",
                    Latitude = 0,
                    Longitude = 0,
                    Country = "TEST",
                    State = "TEST",
                    SetRise = newSetRiseTime
                };


                newSetRiseTime.CityData = newCity;

                solarContext.Cities.Add(newCity);
                solarContext.SetRiseTimes.Add(newSetRiseTime);
                solarContext.SaveChanges();

                System.Console.WriteLine("Data added successfully!");


            }
            else
            {
                System.Console.WriteLine("Already have data - not seeding");
            }

        }



        void AddRoles(RoleManager<IdentityRole> roleManager)
        {

            Console.WriteLine("Adding Roles...");
            
            var tAdmin = CreateAdminRole(roleManager);
            tAdmin.Wait();

            var tUser = CreateUserRole(roleManager);
            tUser.Wait();
        }

        async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        void AddAdmin(UserManager<IdentityUser> userManager)
        {
            var tAdmin = CreateAdminIfNotExists(userManager);
            tAdmin.Wait();
        }

        async Task CreateAdminIfNotExists(UserManager<IdentityUser> userManager)
        {

            var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
            if (adminInDb == null)
            {

                Console.WriteLine("Creating Admin Role...");
                
                var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
                var adminCreated = await userManager.CreateAsync(admin, "admin123");

                if (adminCreated.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}