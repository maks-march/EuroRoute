using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTO.Order;

public record OrderListVm : IMapWith<Domain.Models.Order.Order>
{
    public Guid Id { get; set; }
    public int SpecNumber { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    
    public string StartCity { get; set; } = string.Empty;
    public string EndCity { get; set; } = string.Empty;
    
    public double TotalWeight { get; set; }
    public double TotalVolume { get; set; }
    
    public string PaymentType { get; set; }
    public double MinCost { get; set; }

    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Order.Order, OrderListVm>()
            .ForMember(dest => dest.Status, opt =>
                opt.MapFrom(src => nameof(src.Status)))
            .ForMember(dest => dest.PaymentType, opt =>
                opt.MapFrom(src => nameof(src.Payment.PaymentType)))
            
            .ForMember(dest => dest.StartCity, opt => 
                opt.MapFrom(src => src.RoutePoints.First().City))
            .ForMember(dest => dest.EndCity, opt => 
                opt.MapFrom(src => src.RoutePoints.Last().City))
            
            .ForMember(dest => dest.TotalVolume, opt => 
                opt.MapFrom(src => src.Payloads.Select(p => p.Volume).Sum()))
            .ForMember(dest => dest.TotalWeight, opt => 
                opt.MapFrom(src => src.Payloads.Select(p => p.Volume).Sum()))
            
            .ForMember(dest => dest.MinCost, opt => 
                opt.MapFrom(src => Math.Min(
                    Math.Min(src.Payment.NotTaxedByCard, src.Payment.TaxedByCard), 
                    src.Payment.ByCash)));
    }
}