namespace Domain.Models;

public class File : Entity
{
    public Guid ItemId { get; set; }
    public required string FilePath { get; set; }
    public required Entity Parent { get; set; }
}