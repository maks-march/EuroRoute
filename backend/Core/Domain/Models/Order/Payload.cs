using Domain.Enums;

namespace Domain.Models.Order;

public class Payload : OrderField
{
    public required string Name { get; set; }
    public required double Weight {get; set;}
    public required double Volume {get; set;}
    public int Amount { get; set; } = 1;
    public Wrap Wrap { get; set; } = Wrap.None;
    
}