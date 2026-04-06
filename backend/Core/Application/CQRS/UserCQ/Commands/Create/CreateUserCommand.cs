using MediatR;

namespace Application.CQRS.UserCQ.Commands.Create;

public class CreateUserCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}