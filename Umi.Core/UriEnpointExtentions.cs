using System;

namespace Umi.Core
{
    public static class UriEnpointExtentions
    {
        public static Uri RegisterAsEndpoint(this Uri uri)
        {
            EndpointManager.RegisterEndpoint(new Endpoint(uri));
            return uri;
        }
    }
}
