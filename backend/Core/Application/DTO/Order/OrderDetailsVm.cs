using Application.Common.Mappings;
using Application.CQRS.OrderCQ.Commands.Create;
using AutoMapper;
using Domain.Models.Order;

namespace Application.DTO.Order;

public record OrderDetailsVm : CreateOrderCommand, IMapWith<Domain.Models.Order.OrderEntity>
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; init; }

    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Order.OrderEntity, OrderDetailsVm>()
            .ForMember(dest => 
                dest.Status,opt =>
                opt.MapFrom(src => src.Status.ToString()))
            
            // Настраиваем маппинг для вложенных DTO
            .ForMember(dest => 
                dest.Payment, opt => 
                opt.MapFrom(src => src.Payment))
            .ForMember(dest => 
                dest.Transport, opt => 
                opt.MapFrom(src => src.Transport))
            .ForMember(dest => 
                dest.Payloads, opt => 
                opt.MapFrom(src => src.Payloads))
            .ForMember(dest => 
                dest.RoutePoints, opt => 
                opt.MapFrom(src => src.RoutePoints));
            
        // Также нам нужны маппинги для самих DTO в их доменные аналоги
        profile.CreateMap<Payment, PaymentCreateCommand>()
               .ForMember(dest => 
                   dest.PaymentType,opt => 
                   opt.MapFrom(src => src.PaymentType.ToString()));
        
        profile.CreateMap<Transport, TransportCreateCommand>();

       profile.CreateMap<Payload, PayloadCreateCommand>()
           .ForMember(dest => 
               dest.Wrap,opt => 
               opt.MapFrom(src => src.Wrap.ToString()));

       profile.CreateMap<RoutePoint, RoutePointCreateCommand>();
    }
}