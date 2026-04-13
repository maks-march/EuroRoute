using MediatR;

namespace Application.CQRS.UserCQ.Commands.Update;

public record UpdateUserCommand : IRequest<Guid>
{
    /// <summary>
    /// Идентификатор обновляемого пользователя
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Новое имя пользователя (часть nickname)
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Новая фамилия пользователя (часть nickname)
    /// </summary>
    public string? Surname { get; init; }
}