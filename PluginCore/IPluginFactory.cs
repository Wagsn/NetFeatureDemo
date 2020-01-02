using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    /// <summary>
    /// 插件工厂
    /// </summary>
    public interface IPluginFactory
    {
        /// <summary>
        /// 已加载程序集
        /// </summary>
        List<Assembly> LoadedAssembly { get; }
        //IPlugin GetPlugin(string pluginId);
        //PluginItem GetPluginInfo(string pluginId, bool secret = false);
        //List<PluginItem> GetPluginList(bool secret = false);
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="pluginPath"></param>
        void Load(string pluginPath);
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<bool> Init(PluginCoreContext context);
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<bool> Start(PluginCoreContext context);
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<bool> Stop(PluginCoreContext context);
    }
}
