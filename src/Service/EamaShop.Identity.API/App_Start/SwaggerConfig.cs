using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Identity.API
{
    public class SwaggerConfig
    {
        public static void Configure(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint($"/swagger/v1/swagger.json", AppDomain.CurrentDomain.FriendlyName);
            options.ConfigureOAuth2("swaggerui", "", "", AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
