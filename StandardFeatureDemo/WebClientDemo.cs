using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NetFeatureDemo
{
    class WebClientDemo
    {
        static void Main(string[] args)
        {
            // C#使用WebClient调用接口 https://www.cnblogs.com/liqipiao/p/10999589.html
            Console.Write("download uri: ");
            var uri = Console.ReadLine();

            System.Net.WebClient client = new System.Net.WebClient();

            Console.WriteLine("Download String:");
            var resBytes = client.DownloadData(uri);
            var resStr = Encoding.UTF8.GetString(resBytes);
            Console.WriteLine(resStr);
        }

        /// <summary>
        /// 测试WebClient下载文件
        /// </summary>
        public static void TestDownload()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            Console.Write("download uri: ");
            var inputUri = Console.ReadLine();

            // 在控制台打印下载的文本
            //client.Headers.Add("Conent-Type", "application/json");
            var str2 = client.DownloadString(inputUri);
            Console.WriteLine(string.Join(", ", client.ResponseHeaders.Keys.Cast<string>().ToArray()));
            Console.WriteLine(str2);

            // 下载网络资源到运行文件夹下的files文件夹
            var uri = new UriBuilder(inputUri);
            var baseDic = AppDomain.CurrentDomain.BaseDirectory; // AppContext.BaseDirectory;
            var path = System.IO.Path.Combine(baseDic, $"./files".Replace('/', System.IO.Path.DirectorySeparatorChar));
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var fileName = $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}{(System.IO.Path.HasExtension(uri.Path) ? System.IO.Path.GetExtension(uri.Path) : ".html")}".Replace('/', System.IO.Path.DirectorySeparatorChar);
            var relationName = System.IO.Path.Combine(path, fileName);
            client.DownloadFile(inputUri, relationName);

            // 打开文件浏览器，并选择下载好的文件
            var fullName = System.IO.Path.Combine(baseDic, relationName);
            Console.WriteLine("open explorer and select: " + fullName);
            Process process = new Process();
            process.StartInfo.FileName = "explorer";
            process.StartInfo.Arguments = @"/select," + fullName;
            process.Start();
        }

        /// <summary>
        /// POST WebClient
        /// </summary>
        public static void TestGet()
        {
            Console.Write("download uri: ");
            var inputUri = Console.ReadLine();

            System.Net.WebClient client = new System.Net.WebClient();
            //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //var encoding = Encoding.UTF8.GetBytes()
            var resBytes = client.UploadData(inputUri, "GET", new byte[] { });
            var res = Encoding.UTF8.GetString(resBytes);
            Console.WriteLine(res);
        }

        /// <summary>
        /// 通过WebClient类Post数据到远程地址，需要Basic认证；
        /// 调用端自己处理异常
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="paramStr">name=张三&age=20</param>
        /// <param name="encoding">请先确认目标网页的编码方式</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Request_WebClient(string uri, string method, string paramStr, Encoding encoding, string username, string password)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (string.IsNullOrEmpty(method))
                method = "POST";

            System.Net.WebClient wc = new System.Net.WebClient();

            // 采取POST方式必须加的Header
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            byte[] postData = encoding.GetBytes(paramStr);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                //wc.Credentials = GetCredentialCache(uri, username, password);
                //wc.Headers.Add("Authorization", GetAuthorization(username, password));
            }

            byte[] responseData = wc.UploadData(uri, method, postData); // 得到返回字符流
            return encoding.GetString(responseData);// 解码                  
        }
    }
}
