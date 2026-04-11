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
        order.Transport.Id = Guid.NewGuid();
        foreach (var payload in order.Payloads) payload.Id = Guid.NewGuid();
        foreach (var point in order.RoutePoints) point.Id = Guid.NewGuid();
        order.RoutePoints.First().IsLoad = true;
        order.RoutePoints.Last().IsLoad = false;
        
        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}