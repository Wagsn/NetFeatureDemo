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
        /// 发送请求
        /// </summary>
        /// <typeparam name="T">Response Body 反序列化类型</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        IXkjRestResponse<T> Send<T>(IXkjRestRequest request);
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IXkjRestResponse Send(IXkjRestRequest request);
    }
}
