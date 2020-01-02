using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin
    {
        Task<PluginResultMessage> Init(PluginCoreContext context);
        Task<PluginResultMessage> Start(PluginCoreContext context);
        Task<PluginResultMessage> Stop(PluginCoreContext context);
    }
}
