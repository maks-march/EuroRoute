using Application.DTO.Auth;
using MediatR;

namespace Application.CQRS.AuthCQ.Login;

public record LoginCommand : IRequest<AuthResponse>
{
    /// <summary>
    /// Логин пользователя - почта (скорее всего).
    /// </summary>
    public required string Login { get; init; }
    
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public required string Password { get; init; }
}