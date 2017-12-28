using Microsoft.AspNetCore.Http;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
namespace EamaShop.Client.Identity
{
    internal class ProxyHttpCredentails : ServiceClientCredentials
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ProxyHttpCredentails(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public override void InitializeServiceClient<T>(ServiceClient<T> client)
        {
            base.InitializeServiceClient(client);
        }
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var headers = _httpContextAccessor?.HttpContext?.Request?.Headers;

            if (headers == null)
            {
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }

            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorization[1]);

            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
