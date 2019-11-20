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
            var req = (HttpWebRequest)WebRequest.Create(uri);
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Method = method.ToString();
            req.Timeout = 1000 * 3600;
            // 添加请求头
            if (headers != null) req.Headers.Add(headers);
            try
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                if (method == Method.POST && !String.IsNullOrEmpty(body))
                {
                    using (Stream stream = req.GetRequestStream())
                    {
                        var jsonByte = Encoding.UTF8.GetBytes(body);
                        stream.Write(jsonByte, 0, jsonByte.Length);
                    }
                }
                else
                {
                    req.ContentLength = 0;
                }
                var response = (HttpWebResponse)req.GetResponse();
                using (var myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(myStreamReader.ReadToEnd());
                    response.Close();
                    req.Abort();
                    watch.Stop();
                    return result;
                }
            }
            catch (WebException ex)
            {
                req.Abort();
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

        public IXkjRestResponse<TEntity> Send<TEntity>(IXkjRestRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // 请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(request.Resource);

            req.Method = request.Method.ToString();
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Timeout = 1000 * 3600;
            try
            {
                req.ContentLength = 0;
                var response = Wraper<TEntity>((HttpWebResponse)req.GetResponse());
                req.Abort();
            }
            catch (WebException ex)
            {
                req.Abort();

                var res = (HttpWebResponse)ex.Response;
                if(res != null)
                {
                    var response = Wraper<TEntity>(res);
                    response.ErrorException = ex;
                    return response;
                }
                else
                {
                    return new XkjRestResponse<TEntity>
                    {
                        ErrorException = ex
                    };
                }
            }

            throw new NotImplementedException();
        }

        private void Wraper<TEntity>(IXkjRestResponse<TEntity> response, HttpWebResponse res)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            using (var reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
            {
                var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(reader.ReadToEnd());

                response.StatusCode = res.StatusCode;
                response.StatusDescription = res.StatusCode.ToString();
                response.ContentEncoding = res.ContentEncoding;
                response.ContentLength = res.ContentLength;
                response.ContentType = res.ContentType;
                response.Content = reader.ReadToEnd();
                response.ProtocolVersion = res.ProtocolVersion;

                response.Data = entity;

                res.Close();
                watch.Stop();
                response.Time = watch.ElapsedMilliseconds;
            }
        }
        private IXkjRestResponse<TEntity> Wraper<TEntity>(HttpWebResponse res)
        {
            IXkjRestResponse<TEntity> response = new XkjRestResponse<TEntity>();
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            using (var reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
            {
                var content = reader.ReadToEnd();
                if (response.Data is string resData)
                {
                    resData = content;
                }
                else
                {
                    try
                    {
                        var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(content);
                        response.Data = entity;
                    }
                    catch{ }
                }

                response.StatusCode = res.StatusCode;
                response.StatusDescription = res.StatusCode.ToString();
                response.ContentEncoding = res.ContentEncoding;
                response.ContentLength = res.ContentLength;
                response.ContentType = res.ContentType;
                response.Content = reader.ReadToEnd();
                response.ProtocolVersion = res.ProtocolVersion;


                res.Close();
                watch.Stop();
                response.Time = watch.ElapsedMilliseconds;
            }
            return response;
        }


        public static void Main(string[] args)
        {
            IXkjRestClient client = new XkjRestClient();
            // 如果需要序列化就用泛型方法
            //var data = client.Send<Dictionary<string, string>>(new XkjRestRequest("https://api.github.com/")).Data;
            var req = new XkjRestRequest("https://api.github.com");
            var headers = new Dictionary<string, string>
            {
                //{"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3" },
                //{"Accept-Encoding","gzip, deflate, br" },
                //{"Accept-Language","zh-CN,zh;q=0.9" },
                //{"Cache-Control","max-age=0" },
                //{"User-Agent","XkjRestSharp/0.1.0" }
                //{"Connection", "keep-alive" },
                //{"Host", "api.github.com" },
            };
            req.Headers = headers;
            var res = client.Send(req);
            var resBody = res.Content;
            Console.WriteLine("body: " + resBody);
            Console.WriteLine("error: " + res.ErrorException?.ToString());
        }

        public IXkjRestResponse Send(IXkjRestRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // 请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(request.Resource);

            req.Method = request.Method.ToString();
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Timeout = 1000 * 3600;
            req.UserAgent = "XkjRestSharp/0.1.0";

            // Header
            if (request is XkjRestRequest restRequest)
            {
                if (req.Headers == null) req.Headers = new WebHeaderCollection();
                foreach(var kv in restRequest?.Headers)
                {
                    if(kv.Key.ToLower() == "user-agent")
                    {
                        req.UserAgent = kv.Value;
                    }
                    else if (kv.Key.ToLower() == "accept")
                    {
                        req.Accept = kv.Value;
                    }
                    else if (kv.Key.ToLower() == "connection")
                    {
                        req.Connection = kv.Value;
                    }
                    else if (kv.Key.ToLower() == "host")
                    {
                    }
                    else
                    {
                        req.Headers.Add(kv.Key, kv.Value);
                    }
                }
            }

            try
            {
                req.ContentLength = 0;
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                var res = (HttpWebResponse)req.GetResponse();
                using (var reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                {
                    var response = new XkjRestResponse();
                    response.StatusCode = res.StatusCode;
                    response.StatusDescription = res.StatusCode.ToString();
                    response.ContentEncoding = res.ContentEncoding;
                    response.ContentLength = res.ContentLength;
                    response.ContentType = res.ContentType;
                    response.Content = reader.ReadToEnd();
                    response.ProtocolVersion = res.ProtocolVersion;

                    res.Close();
                    req.Abort();
                    watch.Stop();
                    return response;
                }
            }
            catch(WebException ex)
            {
                req.Abort();

                var res = (HttpWebResponse)ex.Response;
                if (res != null)
                {
                    var response = Wraper<string>(res);
                    response.ErrorException = ex;
                    return response;
                }
                return new XkjRestResponse()
                {
                    StatusCode = res.StatusCode,
                    ErrorException = ex
                };

                //var instan = new TResponse();
                //if (instan is ResponseMessage responseMessage)
                //{
                //    var response = (HttpWebResponse)ex.Response;
                //    responseMessage.Code = ((int)response.StatusCode).ToString();
                //    responseMessage.Message = ex.Message;
                //}
                //return instan;
            }
        }

        public IXkjRestResponse<T> Deserialize<T>(IXkjRestResponse response)
        {
            throw new NotImplementedException();
        }
    }
}
