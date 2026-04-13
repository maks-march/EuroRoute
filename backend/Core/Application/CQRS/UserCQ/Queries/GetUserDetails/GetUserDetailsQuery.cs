using Application.DTO.User;
using MediatR;

namespace Application.CQRS.UserCQ.Queries.GetUserDetails;

public class GetUserDetailsQuery : IRequest<UserDetailsVm>
{
    /// <summary>
    /// Идентификатор желаемого пользователя
    /// </summary>
    public Guid Id { get; set; }
}