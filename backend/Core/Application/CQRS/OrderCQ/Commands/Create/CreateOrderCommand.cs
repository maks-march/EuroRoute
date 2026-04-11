using System.ComponentModel;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Enums;
using Domain.Models.Order;
using MediatR;

namespace Application.CQRS.OrderCQ.Commands.Create;

public record CreateOrderCommand : IRequest<Guid>, IMapWith<Order>
{
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Ready;
    public string About { get; set; } = string.Empty;
    
    public int SpecNumber { get; set; } = 0;
    
    public ICollection<string> Photo { get; set; } = [];
    public PaymentCommandDto Payment { get; set; } = new();
    public TransportCommandDto Transport { get; set; } = new();
    public ICollection<PayloadCommandDto> Payloads { get; set; } = [];
    public ICollection<RoutePointCommandDto> RoutePoints { get; set; } = [];

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOrderCommand, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Updated, opt => opt.Ignore())
            
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
        profile.CreateMap<PaymentCommandDto, Payment>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.OrderId, opt => opt.Ignore());

        profile.CreateMap<TransportCommandDto, Transport>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore());
               //.ForMember(dest => dest.Order, opt => opt.Ignore());

       profile.CreateMap<PayloadCommandDto, Payload>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.OrderId, opt => opt.Ignore());
       //.ForMember(dest => dest.Order, opt => opt.Ignore())

        profile.CreateMap<RoutePointCommandDto, RoutePoints>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore());
        //.ForMember(dest => dest.Order, opt => opt.Ignore());
    }
}
#region inner dto
public record PaymentCommandDto
{
    public PaymentType PaymentType { get; set; }
    public bool IsTaxedByCard { get; set; }
    public bool IsNotTaxedByCard { get; set; }
    public bool IsByCash { get; set; }
    public double TaxedByCard { get; set; } = 0;
    public double NotTaxedByCard { get; set; } = 0;
    public double ByCash { get; set; } = 0;
    public bool IsVisible { get; set; } = false;
    public int PaymentAfterDays { get; set; } = 0;
    public double Prepayment { get; set; } = 0;
    public bool IsPrepaymentByFuel { get; set; } = false;
}

public record PayloadCommandDto
{
    public string Name { get; set; } = string.Empty;
    
    [DefaultValue(1)]
    public double Weight { get; set; }
    
    [DefaultValue(1)]
    public double Volume { get; set; }
    
    [DefaultValue(1)]
    public int Amount { get; set; } = 1;
    public Wrap Wrap { get; set; } = Wrap.None;
}

public record RoutePointCommandDto
{
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    [DefaultValue("00:00:00")]
    public TimeSpan LoadTimeStart { get; set; } = TimeSpan.Zero;
    
    [DefaultValue("00:00:00")]
    public TimeSpan LoadTimeEnd { get; set; } = TimeSpan.Zero;
    public DateTime Date { get; set; } = DateTime.Today;
    public bool IsLoad { get; set; } = false;
}

public record TransportCommandDto
{
    public ICollection<string> BodyType { get; set; } = [];
    public ICollection<string> LoadType { get; set; } = [];
    public ICollection<string> UnloadType { get; set; } = [];
    
    [DefaultValue(1)]
    public int Vehicles { get; set; } = 1;
    public int? TemperatureFrom { get; set; }
    public int? TemperatureTo { get; set; }
    public bool IsCrewFull { get; set; } = false;
    
    [DefaultValue(1)]
    public int Adr { get; set; } = 1;
    public bool IsHitch { get; set; } = false;
    public bool IsPneumaticVehicle { get; set; } = false;
    public bool IsStakes { get; set; } = false;
    public bool IsTir { get; set; } = false;
    public bool IsT1 { get; set; } = false;
    public bool IsCmr { get; set; } = false;
    public bool IsMedicalBook { get; set; } = false;
}
#endregion