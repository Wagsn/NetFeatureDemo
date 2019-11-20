#if CORE20
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Standard20FeatureDemo.Tests
{
    /// <summary>
    /// 测试HttpClient
    /// </summary>
    public class HttpClientTest
    {
        /// <summary>
        /// 测试HttpClient Get请求
        /// </summary>
        /// <returns></returns>
        public static void TestGet()
        {
            var client = new System.Net.Http.HttpClient();

            Console.Write("input uri: ");
            var inputUri = Console.ReadLine();

            var response = client.GetAsync(inputUri).Result;
            Console.WriteLine(response);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
            }
        }

        /// <summary>
        /// 测试HttpClient发送各种方法的请求
        /// </summary>
        public static void TestSend()
        {
            var client = new HttpClient();

            Console.Write("input uri: ");
            var inputUri = Console.ReadLine();

            var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, inputUri)).Result;
            Console.WriteLine(response);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
            }
        }
    }
}
#endif
