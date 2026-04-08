namespace Application.DTO;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    Guid UserId,
    string UserName
);