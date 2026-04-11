using Application.DTO;
using Application.DTO.Auth;
using MediatR;

namespace Application.CQRS.AuthCQ.Login;

public record LoginCommand : IRequest<AuthResponse>
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}