using Application.CQRS.DTO;
using Application.CQRS.UserCQ.Commands.Create;
using Application.CQRS.UserCQ.Commands.Delete;
using Application.CQRS.UserCQ.Commands.Update;
using Application.CQRS.UserCQ.Queries.GetUserDetails;
using Application.CQRS.UserCQ.Queries.GetUserList;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;

namespace WebApi.Controllers;

public class UserController(IMediator mediator, IMapper mapper) 
    : BaseController(mediator, mapper)
{
    [HttpGet("{id}")]
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
    public async Task<ActionResult<UserListVm>> Get()
    {
        var query = new GetUserListQuery();
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromForm] CreateUserCommand command)
    {
        var id = await Mediator.Send(command);
        return Ok(id);
    }
    
    [HttpDelete("{id}")]
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
    public async Task<ActionResult<Guid>> Update([FromForm]UpdateUserDto body)
    {
        var command = Mapper.Map<UpdateUserCommand>(body);
        command.Id = UserId;
        var id = await Mediator.Send(command);
        return Ok(id);
    }
}