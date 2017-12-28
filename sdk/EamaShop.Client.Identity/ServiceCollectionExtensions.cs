using EamaShop.Client.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides method for add <see cref="IIdentityClient"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加身份认证的访问客户端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityClient(this IServiceCollection services, Uri baseUri)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.TryAddSingleton<IIdentityClient>(sp =>
            {
                var httpAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                return new IdentityClient(baseUri, new ProxyHttpCredentails(httpAccessor));
            });

            return services;
        }
    }
}
