namespace Domain.Enums;

public static class RoleMapping
{
    public static string ToString(this Role role)
    {
        return role switch
        {
            Role.Admin => nameof(Role.Admin),
            Role.Manager => nameof(Role.Manager),
            Role.User => nameof(Role.User),
            _ => string.Empty
        };
    }
}