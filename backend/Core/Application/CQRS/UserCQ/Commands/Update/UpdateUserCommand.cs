using MediatR;

namespace Application.CQRS.UserCQ.Commands.Update;

public record UpdateUserCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
}