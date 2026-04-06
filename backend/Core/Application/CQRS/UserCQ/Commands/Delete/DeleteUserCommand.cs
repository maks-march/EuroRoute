using MediatR;

namespace Application.CQRS.UserCQ.Commands.Delete;

public class DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }
}