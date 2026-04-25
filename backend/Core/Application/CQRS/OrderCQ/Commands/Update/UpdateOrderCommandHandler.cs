using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.OrderCQ.Commands.Update;

public class UpdateOrderCommandHandler(IAppDbContext dbContext, IMapper mapper) 
    : IRequestHandler<UpdateOrderCommand, Guid>
{
    public async Task<Guid> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderExists = await dbContext.Orders
            .AnyAsync(o => o.Id == request.Id && o.UserId == request.UserId, cancellationToken);
        if (!orderExists)
        {
            var exists = await dbContext.Orders.AnyAsync(o => o.Id == request.Id, cancellationToken);
            if (!exists) throw new NotFoundException(nameof(OrderEntity), request.Id);
            throw new ForbiddenException(nameof(OrderEntity), request.UserId);
        }
        
        var order = await dbContext.Orders
            .Include(order => order.User)
            .Include(order => order.Payment)
            .Include(order => order.Transport)
            .Include(order => order.Payloads)
            .Include(order => order.RoutePoints)
            .FirstAsync(order => order.Id == request.Id, cancellationToken);
        
        mapper.Map(request, order);
        mapper.Map(request.Payment, order.Payment);
        mapper.Map(request.Transport, order.Transport);
        
        if (request.Payloads != null)
        {
            UpdateCollection(order.Payloads, request.Payloads, order);
        }
        
        if (request.RoutePoints != null)
        {
            UpdateCollection(order.RoutePoints, request.RoutePoints, order);
        }
        
        order.Updated = DateTime.UtcNow;
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return order.Id;
    }

    private void UpdateCollection<T, K>(IList<T> orderCollection, IList<K> newList, OrderEntity order) 
        where T : OrderCollectionField
    {
        for (int i = orderCollection.Count - 1; i >= 0; i--)
        {
            var existing = orderCollection[i];
            if (i < newList.Count)
            {
                mapper.Map(newList[i], existing);
                existing.OrderIndex = i;
            }
            else
            {
                dbContext.Set<T>().Remove(existing);
            }
        }

        if (newList.Count > orderCollection.Count)
        {
            for (int i = orderCollection.Count; i < newList.Count; i++)
            {
                var dto = newList[i];
            
                var newPayload = mapper.Map<T>(dto);
                newPayload.OrderId = order.Id;
                newPayload.OrderIndex = i;
            
                orderCollection.Add(newPayload);
            }
        }
    }
}