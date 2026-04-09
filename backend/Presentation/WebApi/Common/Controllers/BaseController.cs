using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Common.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseController(IMediator mediator, IMapper mapper) : ControllerBase
{
    protected readonly IMapper Mapper = mapper;
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