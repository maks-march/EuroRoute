using Application.DTO;
using Application.Interfaces.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.AuthCQ.Login;

public class LoginCommandHandler(
    IIdentityService identityService,
    IJwtProvider jwtProvider,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Проверяем пароль и получаем DTO пользователя
        var (succeeded, userDto) = await identityService.CheckPasswordAsync(request.Login, request.Password);
        
        if (!succeeded || userDto == null)
            throw new UnauthorizedAccessException("Invalid login or password.");
        
        // 2. Генерируем токены
        var accessToken = jwtProvider.GenerateAccessToken(userDto);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        // 3. Сохраняем Refresh Token
        var appUser = await userManager.FindByIdAsync(userDto.Id.ToString());
        if (appUser == null) throw new InvalidOperationException("User not found after login.");

        appUser.RefreshToken = refreshToken;
        appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(appUser);
        
        // 4. Возвращаем ответ
        return new AuthResponse(accessToken, refreshToken, userDto.Id, userDto.UserName!);
    }
}