using Application.Common.Exceptions;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.CQRS.UserCQ.Queries.GetUserDetails;

public class GetUserDetailsQueryHandler(
    IAppDbContext dbContext, IMapper mapper) 
    : IRequestHandler<GetUserDetailsQuery, UserDetailsVm>
{
    public async Task<UserDetailsVm> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.BusinessUsers.FindAsync([request.Id], cancellationToken: cancellationToken);
        if (user == null || user.Id != request.Id)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }
        return mapper.Map<UserDetailsVm>(user);
    }
}