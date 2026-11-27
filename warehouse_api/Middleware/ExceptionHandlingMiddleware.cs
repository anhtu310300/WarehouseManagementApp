using warehouse_api.Common.Responses;

namespace warehouse_api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException)
            when (context.RequestAborted.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception");

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            var response = new ApiResponse<object?>
            {
                Data = null,
                Message = "An unexpected server error occurred.",
                ErrorMessages = new List<string>(),
                IsSuccess = false
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}