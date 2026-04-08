using Application.DTO;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Persistence.Common.Auth;

namespace Persistence.Common.Mappings;

public class ApplicationUserConverter(UserManager<ApplicationUser> userManager)
    : ITypeConverter<ApplicationUser, ApplicationUserDto>
{
    public ApplicationUserDto Convert(ApplicationUser source, ApplicationUserDto destination, ResolutionContext context)
    {
        var roles = userManager
            .GetRolesAsync(source)
            .GetAwaiter()
            .GetResult()
            .Select(role =>
                {
                    if (Enum.TryParse<Role>(role, out var r))
                        return r;
                    throw new InvalidOperationException($"Unknown role: {role}");
                })
            .ToList();
        
        return new ApplicationUserDto(source.Id, source.UserName ?? string.Empty, roles);
    }
}