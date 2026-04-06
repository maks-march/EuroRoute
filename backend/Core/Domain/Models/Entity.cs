namespace Domain.Models;

public class Entity
{
    public required Guid Id { get; set; }
    public required DateTime Created { get; set; } = DateTime.UtcNow;
    public required DateTime Updated { get; set; } = DateTime.UtcNow;
}