using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    /// <summary>
    /// 插件配置
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public interface IPluginConfig<TConfig>
    {
        //Type ConfigType { get; }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<TConfig> GetConfig(PluginCoreContext context);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        Task<bool> SaveConfig(TConfig cfg);
        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        TConfig GetDefaultConfig(PluginCoreContext context);
        /// <summary>
        /// 配置被更改
        /// </summary>
        /// <param name="context"></param>
        /// <param name="newConfig"></param>
        /// <returns></returns>
        Task<PluginResultMessage> ConfigChanged(PluginCoreContext context, TConfig newConfig);
    }
}
