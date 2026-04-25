namespace Domain.Models.Abstract;

public abstract class HasAuthor : Entity
{
    public Guid UserId { get; set; }
    
    // Навигационное свойство для EF Core
    public User User { get; set; } = null!;
}