using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace XkjRestSharp
{
    /// <summary>
    /// 实现
    /// </summary>
    public class XkjRestResponse : IXkjRestResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public Uri ResponseUri { get; set; }
        public Exception ErrorException { get; set; }
        public Version ProtocolVersion { get; set; }
        public string Server { get; set; }
        public byte[] RawBytes { get; set; }

        public bool IsSuccessful { get => StatusCode == HttpStatusCode.OK; }

        public string Content { get; set; }
        public string ContentEncoding { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public string StatusDescription { get; set; }
        public long Time { get; set; }
    }

    public class XkjRestResponse<T> : XkjRestResponse, IXkjRestResponse<T>
    {
        public XkjRestResponse() { }
        public XkjRestResponse(XkjRestResponse response)
        {
            //this.Content = response.Content;
            //this.ContentEncoding = response.ContentEncoding;
            //this.ContentLength = response.ContentLength;
            //this.ContentType = response.ContentType;
            //this.ErrorException = response.ErrorException;
            //this.ProtocolVersion = response.ProtocolVersion;
            //this.RawBytes = response.RawBytes;
            //this.ResponseUri 
        }

        public T Data { get; set; }
    }
}
