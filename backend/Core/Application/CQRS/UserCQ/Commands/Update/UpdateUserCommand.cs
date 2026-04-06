using MediatR;

namespace Application.CQRS.UserCQ.Commands.Update;

public class UpdateUserCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}