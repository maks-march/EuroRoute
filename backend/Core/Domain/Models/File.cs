using Domain.Models.Abstract;

namespace Domain.Models;

public abstract class FileEntity<TOwner> : CollectionField where TOwner : Entity
{
    public required string FilePath { get; set; }
    public Guid OwnerId { get; set; }
    public TOwner Owner { get; set; }
}

public class OrderPhoto : FileEntity<Order.OrderEntity>;
public class UserAvatar : FileEntity<User>;