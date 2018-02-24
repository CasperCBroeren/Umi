using System;

namespace Umi.Core
{
    public class Endpoint
    {
        private Uri uri;

        public Endpoint(string uriString) : this(new Uri(uriString)) { }        

        public Endpoint(Uri uri)
        {
            this.uri = uri;
           
        }

    }
}
