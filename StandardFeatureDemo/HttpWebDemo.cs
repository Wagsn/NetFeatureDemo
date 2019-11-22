using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NetFeatureDemo
{
    public class HttpWebDemo
    {
        public static void Main(string[] args)
        {
#if CORE20
            Console.Write("download uri: ");
            var uri = Console.ReadLine();

            Console.WriteLine("WebRequest: ");

            var request = (HttpWebRequest)WebRequest.Create(uri);
            var response = (HttpWebResponse)request.GetResponse();
            using(var reader = new StreamReader(response.GetResponseStream()))
            {
                var resStr = reader.ReadToEnd();
                Console.WriteLine(resStr);
            }
#endif
        }
    }
}
