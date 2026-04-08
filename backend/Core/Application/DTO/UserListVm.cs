namespace Application.DTO;

public record UserListVm
{
    public required IList<UserDetailsVm> Users { get; set; }
}