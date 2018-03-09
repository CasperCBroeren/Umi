using System;

namespace Umi.Core
{
    public static class StringEndpointExtentions
    {
        public static string RegisterAsEndpoint(this string str, Action<EndpointConfiguration> configuration = null)
        {
            EndpointManager.RegisterEndpoint(new Endpoint(str, configuration));
            return str;
        }
    }
}
