namespace OrderService.API.Middlewares;

public class GlobalExceptionHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandling> _logger;

    public GlobalExceptionHandling(RequestDelegate next, ILogger<GlobalExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            // Log to a centralized place ...
            // ...

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
        }
    }
}
