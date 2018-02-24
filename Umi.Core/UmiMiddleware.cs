using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Umi.Core
{
    public class UmiMiddleware
    {  
        private readonly RequestDelegate next;
        private readonly UmiMiddlewareOptions options;
        private readonly IViewRenderService viewRenderService;

        public UmiMiddleware(IViewRenderService viewRenderService)
        {
            this.viewRenderService = viewRenderService;
        }

        public UmiMiddleware(RequestDelegate next, UmiMiddlewareOptions options)
        {
            this.next = next;
            this.options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var url = httpContext.Request.Path;
            if (url.StartsWithSegments(options.LocatorUrl))
            {
                var content = await this.viewRenderService.RenderToStringAsync("UmiStatus", EndpointManager.All());
                await httpContext.Response.WriteAsync(content);
            }

            //return this.next(context);
        }

       
    }

    public static class UmiMiddlewareExtentions
    { 

        public static IApplicationBuilder UseUmi(this IApplicationBuilder builder, Action<UmiMiddlewareOptions> configureOptions = null)
        {
            var options = new UmiMiddlewareOptions();
            configureOptions(options);
            return builder.UseMiddleware<UmiMiddleware>(options);
        }
    }
}

