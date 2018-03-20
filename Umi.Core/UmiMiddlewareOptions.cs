using Microsoft.AspNetCore.Http;
using Umi.Core.Authentication;

namespace Umi.Core
{
    public class UmiMiddlewareOptions
    {
        private string locatorUrl = "/umi";
        public string LocatorUrl { get { return locatorUrl; } set { locatorUrl = (value.StartsWith("/") ? value : "/" + value); } } 
        public PathString LocatorAssetUrl => $"{LocatorUrl}/assets";

        public IAuthentication Authentication { get; set; }
        public bool AuthenticationEnabled { get { return Authentication != null; } }
    }
}
