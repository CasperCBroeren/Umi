using System;
using System.Net;

namespace Umi.Core
{
    public class EndpointConfiguration
    {
        public Action PreTest { get; set; }

        public Action PostTest { get; set; }

        private string category;
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                category = (value == null) ? string.Empty : value;
            }
        }

        public HttpStatusCode TestAsSuccessStatusCode { get; set; }
    }
}
