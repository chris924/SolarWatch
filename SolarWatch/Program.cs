using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SolarWatch;
using SolarWatch.Repository;
using SolarWatch.Services.Repository;
using Microsoft.Extensions.Configuration;
using SolarWatch.Authentication;
using SolarWatch.Services.Authentication;


var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
builder.Services.AddSingleton<ISunriseSunsetAPI, SunriseSunsetAPI>();
builder.Services.AddSingleton<IGeoLocatingAPI, GeoLocatingAPI>();
builder.Services.AddSingleton<ISolarRepository, SolarRepository>();
builder.Services.AddDbContext<SolarWatchApiContext>();
builder.Services.AddDbContext<IdentityUsersContext>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
    .AddEntityFrameworkStores<IdentityUsersContext>();



var jwtSettingsConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("jwtSettings.json", optional: true)
    .Build();

var validIssuer = jwtSettingsConfiguration["ValidIssuer"];
var validAudience = jwtSettingsConfiguration["ValidAudience"];
var issuerSigningKey = configuration["JwtSettings:IssuerSigningKey"];



builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        if (issuerSigningKey != null)
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(issuerSigningKey)
                ),
            };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();