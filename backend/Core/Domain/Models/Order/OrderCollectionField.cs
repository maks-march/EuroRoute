namespace Domain.Models.Order;

public abstract class OrderCollectionField : OrderField
{
    public required int OrderIndex { get; set; }
}