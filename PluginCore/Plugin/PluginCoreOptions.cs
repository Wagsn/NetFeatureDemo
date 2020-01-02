using System;
using System.Collections.Generic;
using System.Text;

namespace PluginCore
{
    /// <summary>
    /// 选项配置
    /// </summary>
    public class PluginCoreOptions
    {
        /// <summary>
        /// 如果是相对路径，则以AppContext.BaseDirectory为基准
        /// 如果是绝对路径，就以绝对路径为准
        /// </summary>
        public string PluginPath { get; set; }
    }
}
