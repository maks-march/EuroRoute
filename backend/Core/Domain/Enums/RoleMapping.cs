namespace Domain.Enums;

public static class RoleMapping
{
    public static Role ToEnum(this string role)
    {
        return role switch
        {
            "Admin" => Role.Admin,
            "Manager" => Role.Manager,
            _ => Role.User
        };
    }
    
    public static Role ToEnum(this IList<string> roles)
    {
        if (roles.Contains("Admin")) return Role.Admin;
        if (roles.Contains("Manager")) return Role.Manager;
        return Role.User;
    }
}