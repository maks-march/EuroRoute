using Application.Common.Mappings;
using AutoMapper;
using Domain.Models;

namespace Application.DTO;

public record UserDetailsVm : IMapWith<User>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDetailsVm>()
            .ForMember(vm => vm.Id,
                opt => opt.MapFrom(src => src.Id))
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