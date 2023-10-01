namespace SolarWatch.Authentication;

public interface IAuthService
{
   Task<AuthResult> RegisterAsync(string email, string username, string password);
}