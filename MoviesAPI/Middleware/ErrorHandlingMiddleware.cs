using System.Net;
using System.Text.Json;
using FluentValidation;
using MoviesAPI.Application.DTOs.Common;

namespace MoviesAPI.Middleware
{
    /// <summary>
    /// Global error handling middleware
    /// Catches all unhandled exceptions and returns consistent error responses
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            BaseResponse<object> response;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    response = BaseResponse<object>.Failure(
                        validationException.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                    response.Message = "Validation failed";
                    break;

                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    response = BaseResponse<object>.Failure("Resource not found");
                    break;

                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    response = BaseResponse<object>.Failure("Unauthorized access");
                    break;

                case ArgumentException argumentException:
                    code = HttpStatusCode.BadRequest;
                    response = BaseResponse<object>.Failure(argumentException.Message);
                    break;

                case InvalidOperationException invalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    response = BaseResponse<object>.Failure(invalidOperationException.Message);
                    break;

                default:
                    // Don't expose internal error details in production
                    var errorMessage = _env.IsDevelopment()
                        ? exception.Message
                        : "An error occurred processing your request";

                    response = BaseResponse<object>.Failure(errorMessage);

                    // Log full exception details
                    _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _env.IsDevelopment()
            };

            var result = JsonSerializer.Serialize(response, options);
            return context.Response.WriteAsync(result);
        }
    }

    /// <summary>
    /// Extension method to register the error handling middleware
    /// </summary>
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}


// Extension method for easier middleware registration
public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
