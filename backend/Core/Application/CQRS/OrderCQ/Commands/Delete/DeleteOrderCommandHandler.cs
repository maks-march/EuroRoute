using Application.Common.Exceptions;
using Application.CQRS.OrderCQ.Commands.Update;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.OrderCQ.Commands.Delete;

public class DeleteOrderCommandHandler(IAppDbContext dbContext) 
    : IRequestHandler<DeleteOrderCommand>
{
    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderExists = await dbContext.Orders
            .AnyAsync(o => o.Id == request.Id && o.UserId == request.UserId, cancellationToken);
        if (!orderExists)
        {
            var exists = await dbContext.Orders.AnyAsync(o => o.Id == request.Id, cancellationToken);
            if (!exists) throw new NotFoundException(nameof(Order), request.Id);
            throw new ForbiddenException(nameof(Order), request.UserId);
        }
        var order = await dbContext.Orders.FindAsync([request.Id], cancellationToken) 
            ?? throw new NotFoundException(nameof(Order), request.Id);
        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}