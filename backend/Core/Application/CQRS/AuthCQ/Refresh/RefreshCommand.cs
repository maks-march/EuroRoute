using Application.DTO;
using Application.DTO.Auth;
using MediatR;

namespace Application.CQRS.AuthCQ.Refresh;

public record RefreshCommand : IRequest<AuthResponse>
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}