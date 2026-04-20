using Application.DTO.Order;
using Domain.Enums;
using MediatR;

namespace Application.CQRS.OrderCQ.Queries.GetOrdersList;

/// <summary>
/// Запрос на выдачу заказов
/// </summary>
public record GetOrderListQuery : IRequest<IList<OrderListVm>>
{
    /// <summary>
    /// Фильтр на статус заказа
    /// </summary>
    public OrderStatus? Status { get; set; }
    
    /// <summary>
    /// Город отправки
    /// </summary>
    public string? StartCity { get; set; }
    
    /// <summary>
    /// Город адресат
    /// </summary>
    public string? EndCity { get; set; }
    /// <summary>
    /// Отправка с этого числа
    /// </summary>
    public DateTime? MinStartDate { get; set; }
    /// <summary>
    /// Максимальный вес
    /// </summary>
    public double? MaxWeight { get; set; }
    /// <summary>
    /// Сортировать по
    /// дате - date
    /// номеру заказа - specnumber
    /// весу - weight
    /// стоимости - cost
    /// </summary>
    public string? SortBy { get; set; } 
    /// <summary>
    /// По возрастанию
    /// </summary>
    public bool IsAscending { get; set; } = true;
}