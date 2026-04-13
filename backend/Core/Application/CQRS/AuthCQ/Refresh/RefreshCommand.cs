using Application.DTO.Auth;
using MediatR;

namespace Application.CQRS.AuthCQ.Refresh;

public record RefreshCommand : IRequest<AuthResponse>
{
    /// <summary>
    /// Токен доступа, нужен для авторизации.
    /// </summary>
    public required string AccessToken { get; init; }
    
    /// <summary>
    /// Токен обновления, в паре с токеном доступа позволяет обновить, и токен доступа, и токен обновления.
    /// </summary>
    public required string RefreshToken { get; init; }
}