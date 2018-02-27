using System;

namespace Umi.Core
{
    public class Endpoint
    {
        

        public Endpoint(string uriString) : this(new Uri(uriString)) { }        

        public Endpoint(Uri uri)
        {
            this.Uri = uri;
           
        }

        public Uri Uri { get; set; }
    }
}
