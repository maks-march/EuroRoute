using System.Security.Claims;
using Application.DTO;

namespace Application.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateAccessToken(ApplicationUser user);
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}