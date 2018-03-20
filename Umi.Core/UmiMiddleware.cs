using Microsoft.AspNetCore.Http; 
using System.Net;
using System.Threading.Tasks;
using Umi.Core.Outputer;

namespace Umi.Core
{
    public class UmiMiddleware
    {
        private readonly RequestDelegate next;
        private readonly UmiMiddlewareOptions options;
        private readonly IOutputer jsonOutputer;
        private readonly IOutputer htmlOutputer;
        private readonly IOutputer assetOutputer; 
        private readonly IHttpClient httpClient;

        public UmiMiddleware(RequestDelegate next, 
                            UmiMiddlewareOptions options,
                            JsonOutputer jsonOutputer,
                            HtmlOutputer htmlOutputer,
                            AssetOutputer assetOutputer)
        {
            this.next = next;
            this.options = options; 
            this.jsonOutputer = jsonOutputer;
            this.jsonOutputer.SetOptions(this.options);
            this.htmlOutputer = htmlOutputer;
            this.htmlOutputer.SetOptions(this.options);
            this.assetOutputer = assetOutputer; 
            this.htmlOutputer.SetOptions(this.options);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var url = httpContext.Request.Path;
            if (url.StartsWithSegments(options.LocatorUrl))
            {
                if (!options.AuthenticationEnabled || options.Authentication.IsAuthenticated(httpContext.Request))
                {
                    if (IsJsonRequest(httpContext))
                    {
                        await this.jsonOutputer.WriteOutput(httpContext);
                    }
                    else if (url.StartsWithSegments(options.LocatorAssetUrl))
                    {
                        await this.assetOutputer.WriteOutput(httpContext);
                    }
                    else
                    {
                        await this.htmlOutputer.WriteOutput(httpContext);
                    }
                }
                else
                {
                    if (options.AuthenticationEnabled)
                    {
                        if (!options.Authentication.IsAuthenticated(httpContext.Request) && !options.Authentication.TriesToAuthenticate(httpContext.Request))
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            httpContext.Response.Headers["WWW-Authenticate"] = @"Basic realm=""/umi"" charset=""UTF-8""";
                        }
                        else
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        }
                    }                    
                }
            }
            else
            {
                if (this.next != null)
                {
                    await this.next(httpContext);
                }
            }
        }
         

        private static bool IsJsonRequest(HttpContext httpContext)
        {
            return httpContext.Request.Headers["accept"] == "application/json" ||
                httpContext.Request.Headers["accept"] == "text/javascript";

        }
    } 
}

