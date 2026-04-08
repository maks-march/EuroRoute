using Domain.Enums;

namespace Application.DTO;

public record ApplicationUserDto(
    Guid Id,
    string UserName,
    IList<Role> Roles
);