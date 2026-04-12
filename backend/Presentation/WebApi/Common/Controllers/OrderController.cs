using Application.CQRS.OrderCQ.Commands.Create;
using Application.CQRS.OrderCQ.Queries.GetOrderDetails;
using Application.DTO.Order;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.Order;

namespace WebApi.Common.Controllers;

[Authorize]
public class OrderController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDetailsVm>> Get(Guid id)
    {
        var query = new GetOrderDetailsQuery() { Id = id };
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody]CreateOrderCommand command)
    {
        command.UserId = UserId;
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> Update([FromForm]UpdateOrderDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}