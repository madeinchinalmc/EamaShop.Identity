using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Identity.API
{
    internal class MvcConfig
    {
        public static void Configure(MvcOptions options)
        {
            var cacheProfile = new CacheProfile()
            {
                Duration = 30,
                Location = ResponseCacheLocation.Any,
                NoStore = false,
                VaryByQueryKeys = new[] { "*" }
            };
            options.CacheProfiles.Add("default", cacheProfile);

            options.Filters.Add<GlobalExceptionFilter>();
            options.Filters.Add<DomainExceptionFilter>();
        }
    }
}
