using DonkeyLive.Shared.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DonkeyLive.WebApi.Data;

public class DonkeyDbContext: DbContext
{
    public DonkeyDbContext(DbContextOptions<DonkeyDbContext> options):base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(s=> typeof(IEntityBase).IsAssignableFrom(s) && !s.IsAbstract && !s.IsInterface);

        foreach (var type in types ?? [])
        {
            if (modelBuilder.Model.FindEntityType(type) == null)
            {
                modelBuilder.Model.AddEntityType(type);
            }
        }
        base.OnModelCreating(modelBuilder);
    }
}
