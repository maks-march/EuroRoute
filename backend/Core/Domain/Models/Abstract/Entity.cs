namespace Domain.Models.Abstract;

public abstract class Entity : BaseEntity
{
    public required DateTime Created { get; set; } = DateTime.UtcNow;
    public required DateTime Updated { get; set; } = DateTime.UtcNow;
}

public abstract class BaseEntity
{
    public required Guid Id { get; set; } = Guid.NewGuid();
}