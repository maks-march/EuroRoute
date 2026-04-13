using Application.DTO.Auth;
using MediatR;

namespace Application.CQRS.AuthCQ.Register;

public record RegisterCommand : IRequest<AuthResponse>
{
    /// <summary>
    /// Имя пользователя (часть nickname)
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// Фамилия пользователя (часть nickname)
    /// </summary>
    public required string Surname { get; init; }
    /// <summary>
    /// Логин пользователя - почта (скорее всего).
    /// </summary>
    public required string Login { get; init; }
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public required string Password { get; init; }
}