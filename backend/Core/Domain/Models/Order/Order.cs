using Domain.Enums;

namespace Domain.Models.Order;

public class Order : HasAuthor
{
    public required DateTime StartDate { get; set; }
    public required OrderStatus Status { get; set; }
    
    public int SpecNumber { get; set; }
    public string About { get; set; } = string.Empty;
    public IList<string> Photo { get; set; } = [];
    
    public required Payment Payment { get; set; }
    public required Transport Transport { get; set; }

    public IList<Payload> Payloads { get; set; } = [];
    public IList<RoutePoints> RoutePoints { get; set; } = [];
}