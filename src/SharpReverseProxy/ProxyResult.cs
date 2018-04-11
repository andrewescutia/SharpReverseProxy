using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace SharpReverseProxy {
    public class ProxyResult {
        public ProxyStatus ProxyStatus { get; set; }
        public int HttpStatusCode { get; set; }
        public Uri OriginalUri { get; set; }
        public Uri ProxiedUri { get; set; }
        public TimeSpan Elapsed { get; set; }
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> RequestHeaders { get; set; }
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> ResponseHeaders { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public HttpMethod HttpMethod { get; set; }

    }

}