using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace PluginCore
{
    /// <summary>
    /// 公共插件上下文
    /// </summary>
    public class PluginCoreContext
    {
        /// <summary>
        /// DI容器
        /// </summary>
        public IServiceCollection Services { get; internal set; }
        /// <summary>
        /// 插件根路径
        /// </summary>
        public string PluginBasePath { get; internal set; }

        //public static PluginCoreContext Current { get; private set; }

        public Task<bool> Init()
        {
            string pluginPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugin");
            PluginFactory.Load(pluginPath);
            return PluginFactory.Init(this).Result;
            //return Task.FromResult(true);
        }

        public Task<bool> Start()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Stop()
        {
            return Task.FromResult(true);
        }
    }
}
