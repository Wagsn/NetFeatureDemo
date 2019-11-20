using System;
using System.Collections.Generic;
using System.Text;

namespace Standard20FeatureDemo
{
    /// <summary>
    /// 程序
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Tests.TestWebClient.TestDownload();
            //Tests.WebClientTest.TestGet();
            //Tests.TestHttpClient.TestGet();
            //Tests.TestHttpClient.TestSend();
            //Tests.TestRestClient.Test();
            Tests.WebRequestTest.Test();

            Console.ReadKey();
        }
    }
}
