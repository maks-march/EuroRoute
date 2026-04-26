using Domain.Models.Abstract;

namespace Domain.Models;

public abstract class FileEntity<TOwner> : CollectionField where TOwner : Entity
{
    public required string FilePath { get; set; }
    public required Guid OwnerId { get; set; }
    public required TOwner Owner { get; set; }
}

public class OrderPhoto : FileEntity<Order.OrderEntity>;
public class UserAvatar : FileEntity<User>;