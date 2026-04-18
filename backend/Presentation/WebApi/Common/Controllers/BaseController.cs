using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Common.Controllers;

/// <summary>
/// Базовая настройка контроллеров
/// </summary>
/// <param name="mediator">Медиатор - обрабатывает запросы</param>
[ApiController]
[Route("api/[controller]/[action]")]
public class BaseController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Модифицируем инъекцию из конструктора, как наследуемое поле только для чтения.
    /// </summary>
    protected readonly IMediator Mediator = mediator;
    
    internal Guid UserId
    {
        get
        {
            var identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return User.Identity is { IsAuthenticated: true } && identifier != null
                ? Guid.Parse(identifier)
                : Guid.Empty;
        }
    }
}