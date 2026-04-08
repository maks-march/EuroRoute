using Application.DTO;
using Domain.Models;

namespace Application.Interfaces.Auth;

public interface IIdentityService
{
    Task<(bool Succeeded, string[] Errors, Guid UserId)> CreateUserAsync(
        string userName, string password, string name, string surname);
        
    Task<User?> FindBusinessUserByIdAsync(Guid userId);
    
    Task<ApplicationUserDto?> FindUserByNameAsync(string userName);
    
    Task<(bool Succeeded, ApplicationUserDto? User)> CheckPasswordAsync(string userName, string password);
}