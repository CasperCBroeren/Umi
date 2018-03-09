using System;
using System.Net;

namespace Umi.Core
{
   public class EndpointConfiguration
    {
        public Action PreTest { get; set; }

        public Action PostTest { get; set; }

        public string Category { get; set; }

        public HttpStatusCode TestAsSuccessStatusCode { get; set; }
    }
}
