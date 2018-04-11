using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SharpReverseProxy {
    public class ProxyResultBuilder {
        private ProxyResult _result;
        private DateTime _start;
        
        public ProxyResultBuilder(Uri originalUri) {
            _result = new ProxyResult {
                OriginalUri = originalUri
            };
            _start = DateTime.Now;
        }

        public async Task<ProxyResult> HttpRequest(HttpRequestMessage request)
        {
            _result.HttpMethod = request.Method;

            var headers = request.Headers.ToList();
            
            if (request.Content != null)
            {
                headers.AddRange(request.Content.Headers);

                //TODO: should this be read as other content types?
                var requestBody = await request.Content.ReadAsStringAsync();
                _result.RequestBody = requestBody;
            }

            _result.RequestHeaders = headers;

            return _result;
        }

        public async Task<ProxyResult> HttpResponse(HttpResponseMessage response)
        {
            var headers = response.Headers.ToList();

            if (response.Content != null)
            {
                headers.AddRange(response.Content.Headers);

                var responseBody = await response.Content.ReadAsStringAsync();
                _result.ResponseBody = responseBody;
            }

            _result.ResponseHeaders = headers;
            _result.HttpStatusCode = (int)response.StatusCode;

            return _result;
        }

        public ProxyResult Proxied(Uri proxiedUri, int statusCode) {
            Finish(ProxyStatus.Proxied);
            _result.ProxiedUri = proxiedUri;
            return _result;
        }

        public ProxyResult NotProxied(int statusCode) {
            Finish(ProxyStatus.NotProxied);
            _result.HttpStatusCode = statusCode;
            return _result;
        }

        public ProxyResult NotAuthenticated() {
            Finish(ProxyStatus.NotAuthenticated);
            _result.HttpStatusCode = StatusCodes.Status401Unauthorized;
            return _result;
        }

        private ProxyResult Finish(ProxyStatus proxyStatus) {
            _result.ProxyStatus = proxyStatus;
            _result.Elapsed = DateTime.Now - _start;
            return _result;
        }
    }
}