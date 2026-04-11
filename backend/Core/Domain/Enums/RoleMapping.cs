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
}