using Application.Common.Mappings;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.CQRS.UserCQ.Commands.Update;

public record UpdateUserCommand : IRequest<Guid>, IMapWith<User>
{
    /// <summary>
    /// Идентификатор обновляемого пользователя
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;
    
    /// <summary>
    /// Новое имя пользователя (часть nickname)
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Новая фамилия пользователя (часть nickname)
    /// </summary>
    public string? Surname { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserCommand, User>()
            .ForMember(dest => 
                dest.Id, opt => 
                opt.Ignore())
            .ForAllMembers(opts => 
                opts.Condition((src, dest, srcMember) => 
                    srcMember != null));
    }
}