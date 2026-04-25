using Application.Common.Exceptions;
using Application.DTO.Order;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.OrderCQ.Queries.GetOrderDetails;

public class GetOrderDetailsQueryHandler(
    IAppDbContext dbContext, IMapper mapper) : IRequestHandler<GetOrderDetailsQuery, OrderDetailsVm>
{
    public async Task<OrderDetailsVm> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(order => order.Payment)
            .Include(order => order.Transport)
            .Include(order => order.Payloads
                .OrderBy(p => p.OrderIndex))
            .Include(order => order.RoutePoints
                .OrderBy(r => r.OrderIndex))
            .AsNoTracking()
            .FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken);
        if (order == null || order.Id != request.Id)
        {
            throw new NotFoundException(nameof(OrderEntity), request.Id);
        }
        return mapper.Map<OrderDetailsVm>(order);
    }
}