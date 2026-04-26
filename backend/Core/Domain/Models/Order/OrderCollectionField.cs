using Domain.Models.Abstract;

namespace Domain.Models.Order;

public abstract class OrderCollectionField : OrderField, ICollectionField
{
    public required int OrderIndex { get; set; }
}