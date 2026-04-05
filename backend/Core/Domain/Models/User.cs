namespace Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public required string Surname { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
}