using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO;

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
        var error = string.Empty;
        var details = string.Empty;
        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                error = validationException.Message;
                details = validationException.InnerException?.Message ?? "Validation exception.";
                break;
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case DbUpdateException dbUpdateException:
                code = HttpStatusCode.Conflict;
                error = dbUpdateException.Message;
                details = dbUpdateException.InnerException?.Message ?? "Database constraint violation.";
                break;
            case UnauthorizedAccessException unauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                error = unauthorizedAccessException.Message;
                details = unauthorizedAccessException.InnerException?.Message ?? "Access denied.";
                break;
            case InvalidOperationException invalidOperationException:
                code = HttpStatusCode.BadRequest;
                error = invalidOperationException.Message;
                details = invalidOperationException.InnerException?.Message ?? "Invalid operation.";
                break;
                
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (error == string.Empty)
        {
            error = exception.Message; 
        }

        if (details == string.Empty)
        {
            details = exception.GetType().Name;
        }
        return context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse()
        {
            Error = error, Details = details
        }));
    }
}