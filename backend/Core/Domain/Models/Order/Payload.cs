using Domain.Enums;

namespace Domain.Models.Order;

public class Payload : OrderField
{
    public required int OrderIndex { get; set; }
    public required string Name { get; set; }
    public required double Weight { get; set; } = 1;
    public required double Volume { get; set; } = 1;
    public int Amount { get; set; } = 1;
    public Wrap Wrap { get; set; } = Wrap.None;
    
}