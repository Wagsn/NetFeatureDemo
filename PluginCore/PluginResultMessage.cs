using System;
using System.Collections.Generic;
using System.Text;

namespace PluginCore
{
    /// <summary>
    /// 插件结果信息
    /// </summary>
    public class PluginResultMessage
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public PluginResultMessage()
        {
            Code = PluginResultCodeDefines.SuccessCode;
        }

        public bool IsSuccess() => Code == PluginResultCodeDefines.SuccessCode;
    }

    public class PluginResultMessage<TData> : PluginResultMessage
    {
        public TData Data { get; set; }
    }

    public class PagingPluginResultMessage<Tentity> : PluginResultMessage<List<Tentity>>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }
        //public int PageCount { get; set; }
    }
}
