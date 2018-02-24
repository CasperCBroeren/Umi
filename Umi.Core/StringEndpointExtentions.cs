namespace Umi.Core
{
    public static class StringEndpointExtentions
    {
        public static string RegisterAsEndpoint(this string str)
        {
            EndpointManager.RegisterEndpoint(new Endpoint(str));
            return str;
        }
    }
}
