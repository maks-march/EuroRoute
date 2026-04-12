using Application.DTO.Order;
using MediatR;

namespace Application.CQRS.OrderCQ.Queries.GetOrderDetails;

public record GetOrderDetailsQuery() : IRequest<OrderDetailsVm>
{
    public Guid Id { get; set; }
}