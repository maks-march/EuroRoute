namespace Domain.Models.Order;

public class RoutePoints : OrderField
{
    public required int OrderIndex { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }
    public TimeSpan LoadTimeStart { get; set; }
    public TimeSpan LoadTimeEnd { get; set; }
    public DateTime Date { get; set; }
    public bool IsLoad { get; set; } = false;
}