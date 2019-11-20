using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XkjRestSharp
{
    /// <summary>
    /// XKJ REST Response
    /// </summary>
    public interface IXkjRestResponse
    {
        /// <summary>
        /// HTTP response status code
        /// </summary>
        System.Net.HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// The URL that actually responded to the content (different from request if redirected)
        /// </summary>
        Uri ResponseUri { get; set; }
        /// <summary>
        /// Exceptions thrown during the request, if any.
        /// </summary>
        Exception ErrorException { get; set; }
        /// <summary>
        /// The HTTP protocol version (1.0, 1.1, etc)
        /// </summary>
        Version ProtocolVersion { get; set; }
        /// <summary>
        /// HttpWebResponse.Server
        /// </summary>
        string Server { get; set; }
        /// <summary>
        /// Response content
        /// </summary>
        byte[] RawBytes { get; set; }
        /// <summary>
        /// Whether or not the response status code indicates success
        /// </summary>
        bool IsSuccessful { get; }
        /// <summary>
        /// String representation of response content
        /// </summary>
        string Content { get; set; }
        /// <summary>
        /// Encoding of the response content
        /// </summary>
        string ContentEncoding { get; set; }
        /// <summary>
        /// Length in bytes of the response content
        /// </summary>
        long ContentLength { get; set; }
        /// <summary>
        /// MIME content type of response
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// Description of HTTP status returned
        /// </summary>
        string StatusDescription { get; set; }
    }

    public interface IXkjRestResponse<TData> : IXkjRestResponse
    {
        /// <summary>
        /// Deserialized entity data
        /// </summary>
        TData Data { get; set; }
    }
}
