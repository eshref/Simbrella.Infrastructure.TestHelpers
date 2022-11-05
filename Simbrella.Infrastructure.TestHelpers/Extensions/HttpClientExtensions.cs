using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;


namespace Simbrella.Infrastructure.TestHelpers.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostRequestAsync<T>(this HttpClient client, T request, string requestUri, string
        idempotencyKey = null, int actionOrder = 1, Dictionary<string, string> requestHeaders = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            httpRequest.Headers.Add("idempotency-key", idempotencyKey ?? Guid.NewGuid().ToString());
            httpRequest.Headers.Add("idempotency-action-order", actionOrder.ToString());

            if (requestHeaders != null && requestHeaders.Count != 0)
            {
                foreach ((string headerName, string headerValue) in requestHeaders)
                {
                    httpRequest.Headers.Add(headerName, headerValue);
                }
            }

            return await client.SendAsync(httpRequest).ConfigureAwait(false);
        }
    }
}