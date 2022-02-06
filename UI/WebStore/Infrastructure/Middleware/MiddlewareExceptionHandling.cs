namespace WebStore.Infrastructure.Middleware;

public class MiddlewareExceptionHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MiddlewareExceptionHandling> _logger;

    public MiddlewareExceptionHandling(RequestDelegate next, ILogger<MiddlewareExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            ExceptionHandler(httpContext,exception);
            throw;
        }
    }

    private void ExceptionHandler(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception,"Ошибка при обработке запроса {0}", httpContext.Request.Path);
    }
}