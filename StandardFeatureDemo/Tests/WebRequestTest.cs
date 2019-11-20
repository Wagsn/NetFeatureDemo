using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Standard20FeatureDemo.Tests
{
    /// <summary>
    /// 测试 Web Request
    /// </summary>
    public class WebRequestTest
    {
        /// <summary>
        /// 测试 Web Reuqest 发送请求
        /// </summary>
        public static void Test()
        {
            Console.Write("download uri: ");
            var inputUri = Console.ReadLine();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(inputUri);
            request.Method = "GET";
            //request.ContentType
            //request.ContentLength 
            //request.Headers
            //request.Credentials
            //request.Accept 
            //request.Address
            //request.AllowAutoRedirect
            //request.Date
            //request.UserAgent
            Console.WriteLine("request: "+request);


        }
    }
}
