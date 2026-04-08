using Microsoft.AspNetCore.Identity;

namespace Persistence.Common.Auth;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}