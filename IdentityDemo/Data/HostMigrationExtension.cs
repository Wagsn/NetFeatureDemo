using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.Extensions.Hosting
{
    public static class HostMigrationExtension
    {
        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seedder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    context.Database.Migrate();
                    seedder(context, services);
                    logger.LogInformation($"执行 DbContext {typeof(TContext).Name} Seed 方法成功");
                }
                catch(Exception ex)
                {
                    logger.LogError(ex, $"执行 DbContext {typeof(TContext).Name} Seed 方法失败");
                }
            }
            return host;
        }
    }
}
