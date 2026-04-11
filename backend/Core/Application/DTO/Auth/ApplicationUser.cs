using Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;

namespace Application.DTO.Auth;

public class ApplicationUser : IdentityUser<Guid>, IApplicationUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}