using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseController(IMediator mediator, IMapper mapper) : ControllerBase
{
    protected IMapper Mapper = mapper;
    protected IMediator Mediator = mediator;
    
    internal Guid UserId => User.Identity is not { IsAuthenticated: true } 
        ? Guid.Empty 
        : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}