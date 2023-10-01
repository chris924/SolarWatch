using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services.Authentication.TokenService;

public interface ITokenService
{
    public string CreateToken(IdentityUser user);
}