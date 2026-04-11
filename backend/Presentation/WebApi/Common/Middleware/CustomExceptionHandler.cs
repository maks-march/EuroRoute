using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Common.Middleware;

public class CustomExceptionHandler(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.Errors);
                break;
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case DbUpdateException dbUpdateException:
                code = HttpStatusCode.Conflict;
                result = JsonSerializer.Serialize(new { 
                    error = dbUpdateException.Message, 
                    details = dbUpdateException.InnerException?.Message ?? "Database constraint violation.",
                });
                break;
            case UnauthorizedAccessException unauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                result = JsonSerializer.Serialize(new
                {
                    error = unauthorizedAccessException.Message,
                    details = unauthorizedAccessException.InnerException?.Message ?? "Access denied.",
                });
                break;
            case InvalidOperationException invalidOperationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new
                {
                    error = invalidOperationException.Message,
                    details = invalidOperationException.InnerException?.Message ?? "Invalid operation.",
                });
                break;
                
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { error = exception.Message, type = exception.GetType().Name });
        }
        return context.Response.WriteAsync(result);
    }
}