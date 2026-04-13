using MediatR;

namespace Application.CQRS.UserCQ.Commands.Create;

public record CreateUserCommand : IRequest<Guid>
{
    /// <summary>
    /// Имя пользователя (часть nickname)
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// Фамилия пользователя (часть nickname)
    /// </summary>
    public required string Surname { get; init; }
}