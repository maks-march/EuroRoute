using MediatR;

namespace Application.CQRS.UserCQ.Commands.Delete;

public class DeleteUserCommand : IRequest
{
    /// <summary>
    /// Идентификатор удаляемого пользователя
    /// </summary>
    public Guid Id { get; set; }
}