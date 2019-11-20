using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Standard20FeatureDemo.Tests
{
#if CORE20
    /// <summary>
    /// 测试Nuget包RestClient
    /// </summary>
    public class RestClientTest
    {
        /// <summary>
        /// 测试RestClient的Get请求
        /// </summary>
        public static void Test()
        {
            RestSharp.RestClient client = new RestSharp.RestClient();

            Console.Write("input uri: ");
            var inputUri = Console.ReadLine();

            var resposne = client.Execute<Dictionary<string, string>>(new RestSharp.RestRequest(inputUri), RestSharp.Method.GET);
            if(resposne.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = resposne.Data;
                Console.WriteLine($"{string.Join("\r\n", data.Select(kv => $"{kv.Key}: {kv.Value}"))}");
            }
        }
    }
#endif
}
