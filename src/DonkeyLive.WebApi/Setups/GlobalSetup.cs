using DonkeyLive.WebApi.Middlewares;
using DonkeyLive.WebApi.Services;

namespace DonkeyLive.WebApi.Setups;

public static class GlobalSetup
{
    public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }

    public static void AddGlobalServices(this IServiceCollection services)
    {
        services.AddScoped<UnitOfWork>();
    }
}