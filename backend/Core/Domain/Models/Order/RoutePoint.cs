namespace Domain.Models.Order;

public class RoutePoint : OrderCollectionField
{
    public required string City { get; set; }
    public required string Address { get; set; }
    public TimeSpan LoadTimeStart { get; set; }
    public TimeSpan LoadTimeEnd { get; set; }
    public DateOnly Date { get; set; }
    public bool IsLoad { get; set; } = false;
}