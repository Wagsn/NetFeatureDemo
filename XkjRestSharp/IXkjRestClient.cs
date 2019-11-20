using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XkjRestSharp
{
    /// <summary>
    /// XKJ REST API Client Interface
    /// </summary>
    public interface IXkjRestClient
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        IXkjRestResponse<T> Deserialize<T>(IXkjRestResponse response);
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        IXkjRestResponse<T> Send<T>(IXkjRestRequest request);

        IXkjRestResponse Send(IXkjRestRequest request);
    }
}
