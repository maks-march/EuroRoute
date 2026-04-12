using Application.DTO.Auth;
using MediatR;

namespace Application.CQRS.AuthCQ.Register;

public record RegisterCommand : IRequest<AuthResponse>
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Login { get; init; }
    public required string Password { get; init; }
}