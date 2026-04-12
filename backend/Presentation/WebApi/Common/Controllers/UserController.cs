using Application.CQRS.UserCQ.Commands.Create;
using Application.CQRS.UserCQ.Commands.Delete;
using Application.CQRS.UserCQ.Commands.Update;
using Application.CQRS.UserCQ.Queries.GetUserDetails;
using Application.CQRS.UserCQ.Queries.GetUserList;
using Application.DTO.User;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.User;

namespace WebApi.Common.Controllers;


[Authorize]
public class UserController(IMediator mediator, IMapper mapper) 
    : BaseController(mediator, mapper)
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDetailsVm>> Get(Guid id)
    {
        var query = new GetUserDetailsQuery()
        {
            Id = id
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
    
    [HttpGet]
    public async Task<ActionResult<UserDetailsVm>> GetMe()
    {
        return await Get(UserId);
    }
    
    [HttpGet]
    public async Task<ActionResult<UserListVm>> Get()
    {
        var query = new GetUserListQuery();
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> Post([FromBody] CreateUserCommand command)
    {
        var id = await Mediator.Send(command);
        return Ok(id);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDetailsVm>> Delete(Guid id)
    {
        var query = new DeleteUserCommand()
        {
            Id = id
        };
        await Mediator.Send(query);
        return NoContent();
    }
    
    [HttpPatch]
    public async Task<ActionResult<Guid>> UpdateMe([FromBody]UpdateUserDto body)
    {
        var command = Mapper.Map<UpdateUserCommand>(body);
        command.Id = UserId;
        if (UserId == Guid.Empty)
        {
            return Unauthorized();
        }
        var id = await Mediator.Send(command);
        return Ok(id);
    }
}