using ExpenseTracker.Api.Exceptions;
using System.Text.Json;

namespace ExpenseTracker.Api.Middleware
{
    public sealed class ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }

            catch(AppException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = new
                    {
                        message = ex.Message,
                        statusCode = ex.StatusCode
                    }
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

            catch(Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Internal server error",
                    statusCode = StatusCodes.Status500InternalServerError
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
