using Application.DTO;
using MediatR;

namespace Application.CQRS.AuthCQ.Refresh;

public record RefreshCommand(
    string AccessToken, 
    string RefreshToken
) : IRequest<AuthResponse>;