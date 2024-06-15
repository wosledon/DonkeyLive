using DonkeyLive.Front.Services.Base;
using DonkeyLive.Shared.Domain.Base;
using System.Reflection;

namespace DonkeyLive.Front.Setups;

public static class ServiceSetup
{
    public static void AddHttpServices(this IServiceCollection services)
    {
        var httpServices = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(s => typeof(IHttpService).IsAssignableFrom(s) && !s.IsAbstract && !s.IsInterface);

        foreach(var service in httpServices)
        {
           services.AddScoped(service);
        }
    }
}
