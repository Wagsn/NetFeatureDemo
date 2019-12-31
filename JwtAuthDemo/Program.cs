using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if CoreApp2x
            var host = WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build();
            host.Run();
#endif
#if CoreApp30
            //var host = Host.CreateDefaultBuilder(args)
            //    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
            //host.Run();
#endif
        }
    }
}
