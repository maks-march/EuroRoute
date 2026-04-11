namespace Domain.Models.Order;

public abstract class OrderField : BaseEntity
{
    public Guid OrderId { get; set; }
    public required Order Order { get; set; }
}