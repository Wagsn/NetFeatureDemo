using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginCore
{
    public static class PluginCoreExtension
    {
        // Init
        //configureOptions
        public static PluginCoreBuilder Configure(this IServiceCollection services, Action<PluginCoreOptions> configureOptions)
        {
            var options = new PluginCoreOptions();
            configureOptions(options);

            PluginCoreContext context = new PluginCoreContext();
            context.Services = services;
            context.PluginBasePath = System.IO.Path.Combine(AppContext.BaseDirectory, options.PluginPath);

            context.Init();
            return new PluginCoreBuilder(options);
        }
    }
}
