using MediatR;

namespace Application.CQRS.UserCQ.Commands.Create;

public record CreateUserCommand : IRequest<Guid>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
}