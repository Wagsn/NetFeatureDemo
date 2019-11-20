using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XkjRestSharp
{
    /// <summary>
    /// XKJ REST Request Interface
    /// </summary>
    public interface IXkjRestRequest
    {
        /// <summary>
        /// Determines what HTTP method to use for this request. Supported methods: GET,
        /// POST, PUT, DELETE, HEAD, OPTIONS Default is GET
        /// </summary>
        Method Method { get; set; }
        /// <summary>
        /// The Resource URL to make the request against. Tokens are substituted with UrlSegment
        /// parameters and match by name. Should not include the scheme or domain. Do not
        /// include leading slash. Combined with RestClient.BaseUrl to assemble final URL:
        /// {BaseUrl}/{Resource} (BaseUrl is scheme + domain, e.g. http://example.com)
        /// </summary>
        string Resource { get; set; }

        Dictionary<string, string> Headers { get; set; }
    }
}
