namespace Domain.Enums;

public static class RoleMapping
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string User = "Manager";

    public static string ToHighRole(this IList<string> roles)
    {
        if (roles.Contains(Admin)) return Admin;
        if (roles.Contains(Manager)) return Manager;
        return User;
    }
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

    public static IList<string> ToStringList(this IList<Role> roles)
    {
        return roles
            .Select(role => role.ToString())
            .ToArray();
    }
}