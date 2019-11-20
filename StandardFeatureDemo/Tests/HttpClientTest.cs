#if CORE20
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NetFeatureDemo.Tests
{
    /// <summary>
    /// 测试HttpClient
    /// </summary>
    public class HttpClientTest : TestBase
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

        /// <summary>
        /// TODO 改为通过反射自动调用
        /// </summary>
        /// <param name="args"></param>
        public override void Test(string[] args)
        {
            if (args == null || args.Length == 0) return;

            switch (args[0])
            {
                case "send":
                    TestSend();
                    break;
                case "get":
                    TestGet();
                    break;
                default:
                    Console.WriteLine("没找到测试单元");
                    break;
            }
        }
    }
}
#endif
