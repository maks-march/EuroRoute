using Application.Common.Mappings;
using Application.CQRS.UserCQ.Commands.Update;
using AutoMapper;

namespace WebApi.DTO;

public class UpdateUserDto : IMapWith<UpdateUserCommand>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserDto, UpdateUserCommand>()
            .ForMember(dto => dto.Name, 
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dto => dto.Surname, 
                opt => opt.MapFrom(src => src.Surname));
    }
}