using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarWatch;
using SolarWatch.Repository;
using SolarWatch.Services.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SolarWatch.Authentication;
using SolarWatch.Controllers;
using SolarWatch.Services.Authentication;
using SolarWatch.Services.Authentication.TokenService;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        var configuration = builder.Configuration;

        DotNetEnv.Env.Load();

        var validIssuer = Environment.GetEnvironmentVariable("VALIDISSUERKEY");
        var validAudience = Environment.GetEnvironmentVariable("VALIDAUDIENCEKEY");
        var issuerSigningKey = Environment.GetEnvironmentVariable("ISSUERSIGNINGKEY");

        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTIONSTRING");

        AddDbContext();
        AddServices();
        ConfigureSwagger();
        AddAuthentication();
        AddIdentity();

        var app = builder.Build();
        app.UseCors();



        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            IdentityModelEventSource.ShowPII = true;
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();


        PrepDb.PrepPopulation(app, app.Environment);

        app.Run();


        void AddServices()
        {
            builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();
            builder.Services.AddTransient<ISunriseSunsetAPI, SunriseSunsetAPI>();
            builder.Services.AddTransient<IGeoLocatingAPI, GeoLocatingAPI>();
            builder.Services.AddTransient<ISolarRepository, SolarRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>(_ => new TokenService(configuration));
            


        }


        void ConfigureSwagger()
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "SolarWatch", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        void AddDbContext()
        {
            builder.Services.AddDbContext<SolarWatchApiContext>();
            builder.Services.AddDbContext<IdentityUsersContext>();
        }

        void AddAuthentication()
        {
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.IncludeErrorDetails = true;
                    if (issuerSigningKey != null)
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ClockSkew = TimeSpan.FromMinutes(5),
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = validIssuer,
                            ValidAudience = validAudience,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(issuerSigningKey)
                            )
                        };
                });
        }

        void AddIdentity()
        {
            builder.Services
                .AddIdentityCore<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddRoles<IdentityRole>() 
                .AddEntityFrameworkStores<IdentityUsersContext>();
        }
    }
}


