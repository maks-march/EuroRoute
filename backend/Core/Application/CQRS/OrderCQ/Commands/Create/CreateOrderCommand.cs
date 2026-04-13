using System.ComponentModel;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Enums;
using Domain.Models.Order;
using MediatR;

namespace Application.CQRS.OrderCQ.Commands.Create;

/// <summary>
/// Команда для создания нового заказа
/// </summary>
public record CreateOrderCommand : IRequest<Guid>, IMapWith<Order>
{
    /// <summary>
    /// Идентификатор пользователя, создающего заказ
    /// </summary>
    public Guid UserId { get; set; } = Guid.Empty;

    /// <summary>
    /// Дата начала выполнения заказа
    /// </summary>
    public DateTime StartDate { get; init; } = DateTime.UtcNow.AddDays(1);

    /// <summary>
    /// Статус заказа
    /// </summary>
    [DefaultValue(nameof(OrderStatus.Ready))]
    public string Status { get; init; } = nameof(OrderStatus.Ready);

    /// <summary>
    /// Дополнительная информация о заказе
    /// </summary>
    public string About { get; init; } = string.Empty;
    
    /// <summary>
    /// Номер спецификации
    /// </summary>
    public int SpecNumber { get; init; } = 100;

    /// <summary>
    /// Информация об оплате
    /// </summary>
    public PaymentCommandDto Payment { get; init; } = new();

    /// <summary>
    /// Информация о транспорте
    /// </summary>
    public TransportCommandDto Transport { get; init; } = new();

    /// <summary>
    /// Коллекция грузов
    /// </summary>
    public ICollection<PayloadCommandDto> Payloads { get; init; } = [];

    /// <summary>
    /// Коллекция точек маршрута
    /// </summary>
    public ICollection<RoutePointCommandDto> RoutePoints { get; init; } = [];

    /// <summary>
    /// Настройка маппинга команды в доменную модель заказа
    /// </summary>
    /// <param name="profile">Профиль маппера AutoMapper</param>
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOrderCommand, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Updated, opt => opt.Ignore())
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
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
            .ForMember(dest => 
                dest.Payloads, opt => 
                opt.MapFrom(src => src.Payloads))
            .ForMember(dest => 
                dest.RoutePoints, opt => 
                opt.MapFrom(src => src.RoutePoints));
            
        // Также нам нужны маппинги для самих DTO в их доменные аналоги
        profile.CreateMap<PaymentCommandDto, Payment>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.OrderId, opt => opt.Ignore())
               .ForMember(dest => 
                   dest.PaymentType,opt => 
                   opt.MapFrom(src => Enum.Parse<PaymentType>(src.PaymentType)));
        
        profile.CreateMap<TransportCommandDto, Transport>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore());

       profile.CreateMap<PayloadCommandDto, Payload>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => 
               dest.OrderId, opt => opt.Ignore())
           .ForMember(dest => 
               dest.Wrap,opt => 
               opt.MapFrom(src => Enum.Parse<Wrap>(src.Wrap)));

        profile.CreateMap<RoutePointCommandDto, RoutePoints>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore());
    }
}

#region inner dto

/// <summary>
/// DTO с информацией об оплате
/// </summary>
public record PaymentCommandDto
{
    /// <summary>
    /// Тип оплаты
    /// </summary>
    [DefaultValue(nameof(Domain.Enums.PaymentType.NoNegotiable))]
    public string PaymentType { get; init; } = nameof(Domain.Enums.PaymentType.Request);
    
    /// <summary>
    /// Флаг оплаты налогооблагаемой картой
    /// </summary>
    public bool IsTaxedByCard { get; init; } = true;

    /// <summary>
    /// Флаг оплаты не налогооблагаемой картой
    /// </summary>
    public bool IsNotTaxedByCard { get; init; } = true;

    /// <summary>
    /// Флаг оплаты наличными
    /// </summary>
    public bool IsByCash { get; init; } = true;

    /// <summary>
    /// Сумма оплаты налогооблагаемой картой
    /// </summary>
    public double TaxedByCard { get; init; }

    /// <summary>
    /// Сумма оплаты не налогооблагаемой картой
    /// </summary>
    public double NotTaxedByCard { get; init; }

    /// <summary>
    /// Сумма оплаты наличными
    /// </summary>
    public double ByCash { get; init; }

    /// <summary>
    /// Флаг видимости условий оплаты
    /// </summary>
    public bool IsVisible { get; init; }

    /// <summary>
    /// Количество дней отсрочки платежа
    /// </summary>
    public int PaymentAfterDays { get; init; }

    /// <summary>
    /// Сумма предоплаты
    /// </summary>
    public double Prepayment { get; init; }

    /// <summary>
    /// Флаг предоплаты топливом
    /// </summary>
    public bool IsPrepaymentByFuel { get; init; }
}

/// <summary>
/// DTO с информацией о грузе
/// </summary>
public record PayloadCommandDto
{
    /// <summary>
    /// Наименование груза
    /// </summary>
    public string Name { get; init; } = "Payload";

    /// <summary>
    /// Вес груза
    /// </summary>
    [DefaultValue(1)] public double Weight { get; init; } = 1;

    /// <summary>
    /// Объем груза
    /// </summary>
    [DefaultValue(1)] public double Volume { get; init; } = 1;
    
    /// <summary>
    /// Количество грузовых мест
    /// </summary>
    [DefaultValue(1)]
    public int Amount { get; init; } = 1;
    
    /// <summary>
    /// Тип упаковки груза
    /// </summary>
    [DefaultValue(nameof(Domain.Enums.Wrap.None))]
    public string Wrap { get; init; } = nameof(Domain.Enums.Wrap.None);
}

/// <summary>
/// DTO с информацией о точке маршрута
/// </summary>
public record RoutePointCommandDto
{
    /// <summary>
    /// Город точки маршрута
    /// </summary>
    public string City { get; init; } = "Some city";

    /// <summary>
    /// Адрес точки маршрута
    /// </summary>
    public string Address { get; init; } = "Some address";
    
    /// <summary>
    /// Время начала загрузки/разгрузки
    /// </summary>
    [DefaultValue("00:00:00")]
    public TimeSpan LoadTimeStart { get; init; } = TimeSpan.Zero;
    
    /// <summary>
    /// Время окончания загрузки/разгрузки
    /// </summary>
    [DefaultValue("00:00:00")]
    public TimeSpan LoadTimeEnd { get; init; } = TimeSpan.Zero;

    /// <summary>
    /// Дата прибытия в точку маршрута
    /// </summary>
    public DateTime Date { get; init; } = DateTime.Today.AddDays(1);

    /// <summary>
    /// Флаг, является ли точка погрузочной
    /// </summary>
    public bool IsLoad { get; init; }
}

/// <summary>
/// DTO с информацией о транспорте
/// </summary>
public record TransportCommandDto
{
    /// <summary>
    /// Тип кузова транспортного средства
    /// </summary>
    public ICollection<string> BodyType { get; init; } = [string.Empty];

    /// <summary>
    /// Тип погрузки
    /// </summary>
    public ICollection<string> LoadType { get; init; } = [];

    /// <summary>
    /// Тип разгрузки
    /// </summary>
    public ICollection<string> UnloadType { get; init; } = [];
    
    /// <summary>
    /// Количество транспортных средств
    /// </summary>
    [DefaultValue(1)]
    public int Vehicles { get; init; } = 1;

    /// <summary>
    /// Нижняя граница температурного режима
    /// </summary>
    public int? TemperatureFrom { get; init; }

    /// <summary>
    /// Верхняя граница температурного режима
    /// </summary>
    public int? TemperatureTo { get; init; }

    /// <summary>
    /// Флаг наличия полной бригады
    /// </summary>
    public bool IsCrewFull { get; init; }
    
    /// <summary>
    /// Класс опасности ADR
    /// </summary>
    [DefaultValue(1)]
    public int Adr { get; init; } = 1;

    /// <summary>
    /// Флаг возможности сцепки
    /// </summary>
    public bool IsHitch { get; init; }

    /// <summary>
    /// Флаг пневматического транспортного средства
    /// </summary>
    public bool IsPneumaticVehicle { get; init; }

    /// <summary>
    /// Флаг наличия стоек
    /// </summary>
    public bool IsStakes { get; init; }

    /// <summary>
    /// Флаг наличия TIR (международная перевозка)
    /// </summary>
    public bool IsTir { get; init; }

    /// <summary>
    /// Флаг наличия T1 (транзитная декларация)
    /// </summary>
    public bool IsT1 { get; init; }

    /// <summary>
    /// Флаг наличия CMR (международная транспортная накладная)
    /// </summary>
    public bool IsCmr { get; init; }

    /// <summary>
    /// Флаг наличия медицинской книжки
    /// </summary>
    public bool IsMedicalBook { get; init; }
}

#endregion