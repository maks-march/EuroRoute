using Application.CQRS.OrderCQ.Commands.Create;
using Application.CQRS.OrderCQ.Queries.GetOrderDetails;
using Application.DTO.Order;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.DTO.Order;

namespace WebApi.Common.Controllers;

[Authorize]
public class OrderController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
{
    
    /// <summary>
    /// Получает информацию о заказе по его ID.
    /// </summary>
    /// <param name="id">Уникальный идентификатор заказа.</param>
    /// <returns>DTO заказа.</returns>
    /// <response code="200">Заказ найден.</response>
    /// <response code="400">Невалидные данные.</response>
    /// <response code="401">Не авторизован (токен отсутствует или невалиден).</response>
    /// <response code="404">Заказ с таким ID не найден.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDetailsVm), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDetailsVm>> Get(Guid id)
    {
        var query = new GetOrderDetailsQuery() { Id = id };
        return await Mediator.Send(query);
    }
    /// <summary>
    /// Создает заказ от имени пользователя.
    /// </summary>
    /// <param name="command">DTO создания заказа.</param>
    /// <returns>Id заказа.</returns>
    /// <response code="200">Заказ найден.</response>
    /// <response code="400">Невалидные данные.</response>
    /// <response code="401">Не авторизован (токен отсутствует или невалиден).</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDetailsVm), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
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