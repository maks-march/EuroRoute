using Application.CQRS.AuthCQ.Login;
using Application.CQRS.AuthCQ.Refresh;
using Application.CQRS.AuthCQ.Register;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Common.Controllers;

public class AuthController(IMediator mediator, IMapper mapper) 
    : BaseController(mediator, mapper)
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var response = await Mediator.Send(command);
        HttpContext.Response.Headers.Append("X-Swagger-Token", response.AccessToken);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Refresh([FromBody] RefreshCommand command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }
}