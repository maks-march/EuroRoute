namespace Domain.Models;

public class User : Entity
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    
    // Навигационные свойства
    public ICollection<Truck> Trucks { get; set; } = new List<Truck>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}