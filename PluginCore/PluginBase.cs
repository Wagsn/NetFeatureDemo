using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    /// <summary>
    /// 插件
    /// </summary>
    public abstract class PluginBase<TConfig> : IPlugin, IPluginConfig<TConfig> where TConfig : class
    {
        /// <summary>
        /// 配置更改
        /// </summary>
        /// <param name="context"></param>
        /// <param name="newConfig"></param>
        /// <returns></returns>
        public virtual Task<PluginResultMessage> ConfigChanged(PluginCoreContext context, TConfig newConfig)
        {
            return Task.FromResult(new PluginResultMessage());
        }

        public Task<TConfig> GetConfig(PluginCoreContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract TConfig GetDefaultConfig(PluginCoreContext context);

        public Task<PluginResultMessage> Init(PluginCoreContext context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveConfig(TConfig cfg)
        {
            throw new NotImplementedException();
        }

        public Task<PluginResultMessage> Start(PluginCoreContext context)
        {
            throw new NotImplementedException();
        }

        public Task<PluginResultMessage> Stop(PluginCoreContext context)
        {
            throw new NotImplementedException();
        }
    }
}
