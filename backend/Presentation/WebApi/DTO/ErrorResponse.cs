namespace WebApi.DTO;

/// <summary>
/// Структура ответа с ошибкой
/// </summary>
public record ErrorResponse
{
    /// <summary>
    /// Основной текст ошибки
    /// </summary>
    public string Error { get; set; } = string.Empty;
    /// <summary>
    /// Пояснение
    /// </summary>
    public string Details { get; set; } = string.Empty;
}