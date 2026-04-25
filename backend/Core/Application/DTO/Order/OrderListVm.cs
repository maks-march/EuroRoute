using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTO.Order;

/// <summary>
/// DTO для отображения заказа в коллекции
/// </summary>
public record OrderListVm : IMapWith<Domain.Models.Order.OrderEntity>
{
    /// <summary>
    /// ID заказа
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Номер спецификации
    /// </summary>
    public int SpecNumber { get; init; }

    /// <summary>
    /// Статус заказа
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Дата начала выполнения заказа
    /// </summary>
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// Город отправления (первая точка маршрута)
    /// </summary>
    public string StartCity { get; init; } = string.Empty;

    /// <summary>
    /// Город назначения (последняя точка маршрута)
    /// </summary>
    public string EndCity { get; init; } = string.Empty;

    /// <summary>
    /// Общий вес всех грузов в заказе
    /// </summary>
    public double TotalWeight { get; init; }

    /// <summary>
    /// Общий объем всех грузов в заказе
    /// </summary>
    public double TotalVolume { get; init; }

    /// <summary>
    /// Тип оплаты (наличные, карта с налогом, карта без налога)
    /// </summary>
    public string PaymentType { get; init; } = string.Empty;

    /// <summary>
    /// Минимальная стоимость среди всех способов оплаты
    /// </summary>
    public double MinCost { get; init; }

    /// <summary>
    /// Настройка маппинга между сущностью Order и ViewModel OrderListVm
    /// </summary>
    /// <param name="profile">Профиль AutoMapper для настройки соответствий</param>
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Order.OrderEntity, OrderListVm>()
            .ForMember(dest => dest.Status, opt =>
                opt.MapFrom(src => nameof(src.Status)))
            .ForMember(dest => dest.PaymentType, opt =>
                opt.MapFrom(src => nameof(src.Payment.PaymentType)))
            
            .ForMember(dest => dest.StartCity, opt => 
                opt.MapFrom(src => src.RoutePoints.OrderBy(r => r.OrderIndex).First().City))
            .ForMember(dest => dest.EndCity, opt => 
                opt.MapFrom(src => src.RoutePoints.OrderBy(r => r.OrderIndex).Last().City))
            
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