using System.ComponentModel;
using Application.Common.Mappings;
using Application.CQRS.OrderCQ.Commands.Create;
using AutoMapper;
using Domain.Enums;

namespace WebApi.DTO.Order;

public record CreateOrderDto : CreateOrderCommand, IMapWith<CreateOrderCommand>
{
    [DefaultValue(nameof(OrderStatus.Ready))]
    public new string Status { get; set; } = nameof(OrderStatus.Ready);
    public new PaymentDto Payment { get; set; } = new PaymentDto();
    public new TransportDto Transport { get; set; } = new TransportDto();
    public new ICollection<PayloadDto> Payloads { get; set; } = [];
    public new ICollection<RoutePointDto> RoutePoints { get; set; } = [];
    
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOrderDto, CreateOrderCommand>()
            .ForMember(dest => 
                dest.Status,opt =>
                opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status)))
            
            // Настраиваем маппинг для вложенных DTO
            .ForMember(dest =>
                dest.Payment, opt =>
                opt.MapFrom(src => src.Payment))
            .ForMember(dest =>
                dest.Transport, opt =>
                opt.MapFrom(src => src.Transport))
            .ForMember(dest => dest.Payloads,
                opt =>
                    opt.MapFrom(src => src.Payloads))
            .ForMember(dest =>
                dest.RoutePoints, opt =>
                opt.MapFrom(src => src.RoutePoints));
        
        // Также нам нужны маппинги для самих DTO в их доменные аналоги
        profile.CreateMap<PaymentDto, PaymentCommandDto>()
            .ForMember(dest => 
                dest.PaymentType,opt =>
                opt.MapFrom(src => Enum.Parse<PaymentType>(src.PaymentType)));

        profile.CreateMap<TransportDto, TransportCommandDto>();

        profile.CreateMap<PayloadDto, PayloadCommandDto>()
            .ForMember(dest => 
                dest.Wrap,opt =>
                opt.MapFrom(src => Enum.Parse<Wrap>(src.Wrap)));

        profile.CreateMap<RoutePointDto, RoutePointCommandDto>();
    }
}

public record PayloadDto : PayloadCommandDto
{
    [DefaultValue(nameof(Domain.Enums.Wrap.None))]
    public new string Wrap { get; set; } = nameof(Domain.Enums.Wrap.None);
}

public record PaymentDto : PaymentCommandDto
{
    [DefaultValue(nameof(Domain.Enums.PaymentType.NoNegotiable))]
    public new string PaymentType { get; set; } = nameof(Domain.Enums.PaymentType.NoNegotiable);
}

public record TransportDto : TransportCommandDto;
public record RoutePointDto : RoutePointCommandDto;