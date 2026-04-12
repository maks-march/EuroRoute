using Application.Interfaces;
using AutoMapper;
using Domain.Models.Order;
using MediatR;

namespace Application.CQRS.OrderCQ.Commands.Create;

public class CreateOrderCommandHandler(IAppDbContext dbContext, IMapper mapper) 
    : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = mapper.Map<Order>(request);
        
        order.Id = Guid.NewGuid();
        order.Created = DateTime.UtcNow;
        order.Updated = DateTime.UtcNow;
        
        order.Payment.Id = Guid.NewGuid();
        order.Payment.OrderId = order.Id;
        order.Transport.Id = Guid.NewGuid();
        order.Transport.OrderId = order.Id;
        foreach (var payload in order.Payloads)
        {
            payload.Id = Guid.NewGuid();
            payload.OrderId = order.Id;
        }
        
        order.RoutePoints.First().IsLoad = true;
        foreach (var point in order.RoutePoints)
        {
            point.Id = Guid.NewGuid();
            point.OrderId = order.Id;
        }
        
        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}