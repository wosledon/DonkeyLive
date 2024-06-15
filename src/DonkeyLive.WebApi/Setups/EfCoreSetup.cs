using Microsoft.EntityFrameworkCore;

namespace DonkeyLive.WebApi.Setups;

public static class EfCoreSetup
{
    public static void AddEfCore<TDbContext>(this IServiceCollection services, IConfiguration configuration)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(opts =>
        {
            opts.UseSqlite($"Data Source={configuration.GetSection("DefaultConnection").Value}", setup =>
            {
                setup.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

            });
            opts.UseLazyLoadingProxies();
        });
    }

    public static void UseEfCore<TDbContext>(this IApplicationBuilder app, Action<EfCoreOption> action)
        where TDbContext : DbContext
    {
        var option = new EfCoreOption();
        action(option);

        using var scoped = app.ApplicationServices.CreateScope();
        var db = scoped.ServiceProvider.GetRequiredService<TDbContext>();

        try
        {
            if(option.Reset)
            {
                db.Database.EnsureDeleted();
            }

            db.Database.EnsureCreated();
        }
        catch(Exception)
        {

        }
    }

    public class EfCoreOption
    {
        public bool Reset { get; set; } = false;
    }
}
