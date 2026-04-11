namespace Application.DTO.User;

public record UserListVm
{
    public required IList<UserDetailsVm> Users { get; init; }
}