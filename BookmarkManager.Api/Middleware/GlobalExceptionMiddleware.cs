using BookmarkManager.Api.Exceptions;
using System.Text.Json;

namespace BookmarkManager.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private static async Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new { status = statusCode, message };
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);  // przepuść request dalej
            }
            catch (AppException ex)
            {

                _logger.LogWarning("App error: {Code} {Message}", ex.StatusCode, ex.Message);
                await WriteResponse(context, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                // Nieznany błąd — to jest bug, logujemy pełne szczegóły
                _logger.LogError(ex, "Unhandled exception");
                await WriteResponse(context, 500, "An unexpected error occurred");

            }
        }
    }
    
}

