using DonkeyLive.Shared.Helpers;
using DonkeyLive.WebApi.Helpers;
using System.Net;
using System.Text.Json;

namespace DonkeyLive.WebApi.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            LogHelper.LogError<GlobalExceptionMiddleware>("An error occurred", ex);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse
            {
                Data = ex.StackTrace
            };

            var result = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(result);
        }
    }
}
