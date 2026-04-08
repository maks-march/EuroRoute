using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.CQRS.UserCQ.Commands.Delete;

public class DeleteUserCommandHandler(IAppDbContext dbContext) 
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.BusinessUsers.FindAsync([request.Id], cancellationToken: cancellationToken);
        if (user == null || user.Id != request.Id)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }
        
        dbContext.BusinessUsers.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}