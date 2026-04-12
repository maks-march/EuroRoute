using Application.Common.Mappings;
using Application.CQRS.UserCQ.Commands.Update;
using AutoMapper;

namespace WebApi.DTO.User;

public class UpdateUserDto : IMapWith<UpdateUserCommand>
{
    public string? Name { get; set; } = null;
    public string? Surname { get; set; } = null;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserDto, UpdateUserCommand>()
            .ForMember(dto => dto.Name, 
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dto => dto.Surname, 
                opt => opt.MapFrom(src => src.Surname));
    }
}