using Domain.Enums;

namespace Domain.Models;

public class User : Entity
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    
    public Role Role { get; set; } = Role.User;
    
    public string? RefreshToken { get; set; } 
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public ICollection<Truck> Trucks { get; set; } = new List<Truck>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}