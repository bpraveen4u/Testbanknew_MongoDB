using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace TestBank.API.WebHost.Filters
{
    public class HttpsRequiredDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.RequestUri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                //var response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "HTTPS Request required for the security reasons.");
                //return Task.Factory.FromAsync(response);
                return Task<HttpResponseMessage>.Factory.StartNew(() =>
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, "HTTPS Request required for the security reasons.");
                });
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}