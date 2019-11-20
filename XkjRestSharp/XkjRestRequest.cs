using System;
using System.Collections.Generic;
using System.Text;

namespace XkjRestSharp
{
    public class XkjRestRequest : IXkjRestRequest
    {
        public Method Method { get; set; } = Method.GET;
        public string Resource { get; set; }

        public XkjRestRequest(string resource, Method method) : this(resource)
        {
            this.Method = method;
        }

        public XkjRestRequest(string resource)
        {
            this.Resource = resource;
        }
    }
}
