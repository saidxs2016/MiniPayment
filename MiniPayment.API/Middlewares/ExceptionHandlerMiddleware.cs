using FluentValidation;
using System.Diagnostics;

namespace MiniPayment.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var spw = Stopwatch.StartNew();
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (ValidationException ex)
        {            
            _logger.LogError(ex, ex.Message);
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "text/plain";
            await httpContext.Response.WriteAsync(ex.Message);
        }


        catch (Exception ex)
        {            
            _logger.LogError(ex, ex.Message);
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "text/plain";
            await httpContext.Response.WriteAsync(ex.Message);
            
        }
        finally
        {
        }

    }

}

public static class ExceptionMiddleware
{
    public static WebApplication UseExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        return app;
    }
}
