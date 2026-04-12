using MediatR;

namespace Application.CQRS.UserCQ.Commands.Update;

public record UpdateUserCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; init; }
    public string? Surname { get; init; }
}