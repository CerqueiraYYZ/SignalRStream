using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SignalRR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls("http://localhost:5001", "http://192.168.11.71:5001");
                    webBuilder.ConfigureLogging(logging =>
                     {
                         logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                         logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                         
                         logging.AddConsole();
                     });
                });

    }
}
