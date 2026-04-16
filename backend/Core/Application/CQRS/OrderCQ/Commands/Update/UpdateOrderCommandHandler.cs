using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.OrderCQ.Commands.Update;

public class UpdateOrderCommandHandler(IAppDbContext dbContext, IMapper mapper) 
    : IRequestHandler<UpdateOrderCommand, Guid>
{
    public async Task<Guid> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(order => order.Payment)
            .Include(order => order.Transport)
            .Include(order => order.Payloads)
            .Include(order => order.RoutePoints)
            .FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken);
        
        if (order == null || order.Id != request.Id)
            throw new NotFoundException(nameof(order), request.Id);
        if (order.UserId != request.UserId)
            throw new UnauthorizedAccessException($"Order with Id = {order.Id} is not does not belong to current user.");
        
        mapper.Map(request, order);
        // for (var index = 0; index < order.Payloads.Count; index++)
        // {
        //     var payload = order.Payloads[index];
        //     payload.Id = Guid.NewGuid();
        //     payload.OrderId = order.Id;
        //     payload.OrderIndex = index;
        // }
        //
        // order.RoutePoints[0].IsLoad = true;
        // for (var index = 0; index < order.RoutePoints.Count; index++)
        // {
        //     var point = order.RoutePoints[index];
        //     point.Id = Guid.NewGuid();
        //     point.OrderId = order.Id;
        //     point.OrderIndex = index;
        // }
        order.Updated = DateTime.UtcNow;
        
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}