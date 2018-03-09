using System;

namespace Umi.Core
{
    public static class UriEnpointExtentions
    {
        public static Uri RegisterAsEndpoint(this Uri uri, Action<EndpointConfiguration> configuration = null)
        {
            EndpointManager.RegisterEndpoint(new Endpoint(uri, configuration));
            return uri;
        }
    }
}
