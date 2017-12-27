using EamaShop.Identity.API;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EamaShop.Service.Test.Base
{
    internal class BaseTests
    {
        protected IServiceProvider RootProvider { get; }
        public BaseTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var env = new HostingEnvironment()
            {
                ApplicationName = "EamaShop.Identity.API",
                EnvironmentName = "Development"
            };
            var startuper = new Startup(configuration, env);

            var service = new ServiceCollection();
            startuper.ConfigureServices(service);

            RootProvider = service.BuildServiceProvider();
        }
    }
}
