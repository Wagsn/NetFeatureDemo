using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace XkjRestSharp
{
    /// <summary>
    /// XKJ Rest API Client
    /// </summary>
    public class XkjRestClient : IXkjRestClient
    {
        /// <summary>
        /// 基础地址（每一个RestClient独自拥有）
        /// <protocol>://<host>(:<port>)?
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// HTTP GET 请求（for XKJ API）
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="cmd">地址（如果BaseUrl不为空则作为请求地址的前缀）</param>
        /// <param name="querys">查询</param>
        /// <returns></returns>
        public TResponse Get<TResponse>(string cmd, Dictionary<string, object> querys = null) where TResponse : class, new()
        {
            var url = string.IsNullOrEmpty(BaseUrl) ? cmd : $"{BaseUrl.TrimEnd('/')}/{cmd.TrimStart('/')}";
            return Execute<TResponse>(url, Method.GET, null);
        }

        public static TResponse Get<TResponse>(string url, NameValueCollection querys = null, NameValueCollection headers = null) where TResponse : class, new()
        {
            return new TResponse();
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url">域名端口路由</param>
        /// <param name="method">请求方式</param>
        /// <param name="body">请求的BODY参数</param>
        /// <returns></returns>
        public static TResponse Execute<TResponse>(string url, Method method, string body, NameValueCollection querys = null, NameValueCollection headers = null)
            where TResponse : class, new()
        {
            var queryString = "";
            if (querys != null)
            {
                var keys = querys.AllKeys;
                for (var i = 0; i < keys.Length; i++)
                {
                    if (i > 0) queryString += "&";
                    queryString += $"{keys[i]}={querys[keys[i]]}";
                }
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                queryString = "?" + queryString;
            }
            var uri = new Uri(url + queryString);
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.ContentType = "application/json";
            webRequest.Accept = "application/json";
            webRequest.Method = method.ToString();
            webRequest.Timeout = 1000 * 3600;
            // 添加请求头
            if (headers != null) webRequest.Headers.Add(headers);
            try
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                if (method == Method.POST && !String.IsNullOrEmpty(body))
                {
                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        var jsonByte = Encoding.UTF8.GetBytes(body);
                        stream.Write(jsonByte, 0, jsonByte.Length);
                    }
                }
                else
                {
                    webRequest.ContentLength = 0;
                }
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                using (var myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(myStreamReader.ReadToEnd());
                    webResponse.Close();
                    webRequest.Abort();
                    watch.Stop();
                    return result;
                }
            }
            catch (WebException ex)
            {
                webRequest.Abort();
                var instan = new TResponse();
                if (instan is ResponseMessage responseMessage)
                {
                    var response = (HttpWebResponse)ex.Response;
                    responseMessage.Code = ((int)response.StatusCode).ToString();
                    responseMessage.Message = ex.Message;
                }
                return instan;
            }
        }

        /// <summary>
        /// 创建URL，合并Query
        /// </summary>
        /// <param name="url"></param>
        /// <param name="qs"></param>
        /// <returns></returns>
        public static string CreateUrl(string url, NameValueCollection qs)
        {
            if (qs != null && qs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                List<string> kl = qs.AllKeys.ToList();
                foreach (string k in kl)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("&");
                    }
                    sb.Append(k).Append("=");
                    if (!String.IsNullOrEmpty(qs[k]))
                    {
                        sb.Append(System.Web.HttpUtility.UrlEncode(qs[k]));
                    }
                }
                if (url.Contains("?"))
                {
                    url = url + "&" + sb.ToString();
                }
                else
                {
                    url = url + "?" + sb.ToString();
                }
            }
            return url;
        }

        public IXkjRestResponse<T> Execute<T>(IXkjRestRequest request) where T : new()
        {
            var res = (IXkjRestResponse<T>)Execute(request);
            res.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res.Content);
            return res;
        }

        public static void Test()
        {
            IXkjRestClient client = new XkjRestClient();
            // 如果需要序列化就用泛型方法
            var data = client.Execute<Dictionary<string, string>>(new XkjRestRequest("https://api.github.com/")).Data;
            var str = client.Execute(new XkjRestRequest("https://api.github.com/")).Content;
        }

        public IXkjRestResponse Execute(IXkjRestRequest request)
        {
            // 请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(request.Resource);

            req.Method = request.Method.ToString();

            throw new NotImplementedException();
        }
    }
}
