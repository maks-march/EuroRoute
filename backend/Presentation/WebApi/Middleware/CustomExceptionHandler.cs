using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Middleware;

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
                    error = "Database constraint violation.", 
                    details = dbUpdateException.InnerException?.Message ?? dbUpdateException.Message,
                });
                break;
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }
        return context.Response.WriteAsync(result);
    }
}