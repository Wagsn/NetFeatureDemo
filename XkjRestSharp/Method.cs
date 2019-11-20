using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XkjRestSharp
{
    /// <summary>
    /// 不同目标框架可能没有HttpMethod
    /// </summary>
    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE,
        HEAD,
        OPTIONS,
        PATCH,
        COPY,
    }

    public static class MethondExtension
    {
#if CORE20
        public static System.Net.Http.HttpMethod ToHttpMethond(this Method method)
        {
            return new System.Net.Http.HttpMethod(method.ToString());
        }
#endif
    }
}
