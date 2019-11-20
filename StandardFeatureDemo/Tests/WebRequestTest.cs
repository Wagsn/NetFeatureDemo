using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NetFeatureDemo.Tests
{
    /// <summary>
    /// 测试 Web Request
    /// </summary>
    public class WebRequestTest : TestBase
    {
        /// <summary>
        /// 测试 Web Reuqest 发送请求
        /// </summary>
        public override void Test(string[] args)
        {
            Console.Write("download uri: ");
            var inputUri = Console.ReadLine();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(inputUri);
            request.Method = "GET";
            request.Timeout = 1000 * 3600;
            request.ContentType = "application/json";
            request.ContentLength = 0;
#if CORE20
            request.Date = DateTime.Now;
#endif
            //request.ContentLength 
            //request.Headers
            //request.Credentials
            //request.Accept 
            //request.Address
            //request.AllowAutoRedirect
            //request.UserAgent
            Console.WriteLine("request: "+request);
            try
            {
                var response = (HttpWebResponse) request.GetResponse();
                Console.WriteLine("resposne: " + response);
                using(StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    Console.WriteLine("resposne body: " + reader.ReadToEnd());
                    response.Close();
                    request.Abort();
                }
            }
            catch(WebException ex)
            {
                Console.WriteLine("error: " + ex.ToString());
                var response = (HttpWebResponse)ex.Response;
                Console.WriteLine("response: " + response?.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine("error: " + ex.ToString());
            }
        }
    }
}
