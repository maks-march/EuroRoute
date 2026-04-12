namespace WebApi.DTO;

public record ErrorResponse
{
    public string Error { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}