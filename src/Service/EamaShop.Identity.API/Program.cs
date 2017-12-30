using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace EamaShop.Identity.API
{
    /// <summary>
    /// entry point class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// entry point method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        /// <summary>
        /// core
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(ConfigureContext)
                .UseStartup<Startup>()
                .Build();

        private static void ConfigureContext(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            // add zookeeper?
            //builder.AddZookeeper();
            if (context.HostingEnvironment.WebRootPath == null)
            {
                Console.WriteLine("Null value of web root path not configured");
                context.HostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory() + "../wwwroot");
            }
            if (context.HostingEnvironment.WebRootFileProvider == null)
            {
                Console.WriteLine("No web root file provider are available to perform action");
                context.HostingEnvironment.WebRootFileProvider = new PhysicalFileProvider(context.HostingEnvironment.WebRootPath);
            }
        }
    }
}
