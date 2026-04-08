namespace Application.Interfaces.Auth;

public interface IApplicationUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}