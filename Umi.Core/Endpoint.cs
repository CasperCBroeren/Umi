using System;
using System.Net;

namespace Umi.Core
{
    public class Endpoint
    { 
        public Endpoint(string uriString, Action<EndpointConfiguration> configure) : this(new Uri(uriString), configure) { }        

        public Endpoint(Uri uri, Action<EndpointConfiguration> configure)
        {
            this.Uri = uri;
            this.TestConfiguration = new EndpointConfiguration()
            {
                TestAsSuccessStatusCode = HttpStatusCode.Accepted
            };
            if (configure != null)
            {
                configure(this.TestConfiguration);
            }
        }

        public EndpointConfiguration TestConfiguration { get; set; }

        public Uri Uri { get; set; }
    }
}
