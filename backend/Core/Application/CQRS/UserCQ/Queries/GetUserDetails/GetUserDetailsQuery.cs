using Application.DTO;
using MediatR;

namespace Application.CQRS.UserCQ.Queries.GetUserDetails;

public class GetUserDetailsQuery : IRequest<UserDetailsVm>
{
    public Guid Id { get; set; }
}