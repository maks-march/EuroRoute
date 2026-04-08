using Application.Common.Exceptions;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.UserCQ.Queries.GetUserList;

public class GetUserListQueryHandler(
    IAppDbContext dbContext, IMapper mapper) 
    : IRequestHandler<GetUserListQuery, UserListVm>
{
    public async Task<UserListVm> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users = await dbContext.BusinessUsers
            .ProjectTo<UserDetailsVm>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        if (users == null || users.Count == 0)
        {
            throw new NotFoundException(nameof(User), request);
        }

        return new () { Users = users };
    }
}