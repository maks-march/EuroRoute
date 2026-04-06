using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.CQRS.UserCQ.Commands.Update;

public class UpdateUserCommandHandler(IAppDbContext dbContext) 
    : IRequestHandler<UpdateUserCommand, Guid>
{
    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.Id], cancellationToken: cancellationToken);
        if (user == null || user.Id != request.Id)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }
        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Updated = DateTime.Now;
        
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}