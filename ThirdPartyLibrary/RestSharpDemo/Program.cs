using System;

namespace ThirdPartyLibrary.RestSharpDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RestSharpDemo:");
            var client = new RestSharp.RestClient("https://api.github.com");
            var response = client.Execute(new RestSharp.RestRequest(RestSharp.Method.GET));
            Console.WriteLine(response.Content);
        }
    }
}
