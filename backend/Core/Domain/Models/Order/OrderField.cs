using Domain.Models.Abstract;

namespace Domain.Models.Order;

public abstract class OrderField : BaseEntity
{
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; }
}