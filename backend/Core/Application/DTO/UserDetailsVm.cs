using Application.Common.Mappings;
using AutoMapper;
using Domain.Models;

namespace Application.DTO;

public record UserDetailsVm : IMapWith<User>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDetailsVm>()
            .ForMember(vm => vm.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(vm => vm.Surname,
                opt => opt.MapFrom(src => src.Surname))
            .ForMember(vm => vm.Created,
                opt => opt.MapFrom(src => src.Created))
            .ForMember(vm => vm.Updated,
                opt => opt.MapFrom(src => src.Updated));
    }
}