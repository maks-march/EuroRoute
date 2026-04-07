namespace Application.CQRS.DTO;

public class UserListVm
{
    public required IList<UserDetailsVm> Users { get; set; }
}