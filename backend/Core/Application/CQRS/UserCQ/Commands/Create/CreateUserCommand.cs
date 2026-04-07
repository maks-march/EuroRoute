using MediatR;

namespace Application.CQRS.UserCQ.Commands.Create;

public class CreateUserCommand : IRequest<Guid>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
}