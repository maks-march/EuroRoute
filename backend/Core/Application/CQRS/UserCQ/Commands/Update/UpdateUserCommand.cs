using MediatR;

namespace Application.CQRS.UserCQ.Commands.Update;

public class UpdateUserCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}