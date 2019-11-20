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
        /// 执行请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        IXkjRestResponse<T> Execute<T>(IXkjRestRequest request) where T : new();

        IXkjRestResponse Execute(IXkjRestRequest request);
    }
}
