using Application.Common.Mappings;
using Application.CQRS.OrderCQ.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.DTO;

/// <summary>
/// DTO для создания заказа с поддержкой загрузки файлов и сложных данных в формате JSON
/// </summary>
public record PhotoDto : IMapWith<CreateOrderCommand>
{
    /// <summary>
    /// Список фотографий, прикрепленных к заказу
    /// </summary>
    /// <remarks>
    /// Поддерживаемые форматы: JPEG, PNG, GIF
    /// </remarks>
    [FromForm(Name = "photos")]
    public List<IFormFile>? Photos { get; set; } = [];
}