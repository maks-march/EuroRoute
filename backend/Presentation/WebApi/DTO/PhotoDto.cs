using Microsoft.AspNetCore.Mvc;

namespace WebApi.DTO;

/// <summary>
/// DTO для создания заказа с поддержкой загрузки файлов и сложных данных в формате JSON
/// </summary>
public record PhotoDto
{
    /// <summary>
    /// Список фотографий, прикрепленных к заказу
    /// </summary>
    /// <remarks>
    /// Поддерживаемые форматы: JPEG, PNG, GIF
    /// </remarks>
    [FromForm(Name = "photos")]
    public IFormFile[]? Photos { get; set; } = [];
}