using Microsoft.AspNetCore.Http;

namespace Umi.Core
{
    public class UmiMiddlewareOptions
    {
        private string locatorUrl = "/umi";
        public string LocatorUrl { get { return locatorUrl; } set { locatorUrl = (value.StartsWith("/") ? value : "/" + value); } } 
        public PathString LocatorAssetUrl => $"{LocatorUrl}/assets";
    }
}
