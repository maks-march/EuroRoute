using Application.DTO.Auth;
using Domain.Models;

namespace Application.Interfaces.Auth;

public interface IIdentityService
{
    Task<(bool Succeeded, string[] Errors, Guid UserId)> CreateUserAsync(
        string userName, string password, string name, string surname);
        
    Task<User?> FindBusinessUserByIdAsync(Guid userId);
    
    Task<ApplicationUser?> FindUserByUsernameAsync(string userName);
    
    Task<(bool Succeeded, ApplicationUser? User)> CheckPasswordAsync(string userName, string password);
}