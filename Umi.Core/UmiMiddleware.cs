using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Umi.Core
{
    public class UmiMiddleware
    {
        private readonly RequestDelegate next;
        private readonly UmiMiddlewareOptions options;
        private readonly IViewRenderService viewRenderService;
        private readonly ITempDataProvider tempDataProvider;
        private readonly IRazorViewEngine razorViewEngine;
        private Stream template;

        public UmiMiddleware(RequestDelegate next, UmiMiddlewareOptions options, 
            IViewRenderService viewRenderService, ITempDataProvider tempDataProvider, IRazorViewEngine razorViewEngine)
        {
            this.next = next;
            this.options = options;
            this.viewRenderService = viewRenderService; 
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var url = httpContext.Request.Path;
            if (url.StartsWithSegments(options.LocatorUrl))
            {

                var content = await this.viewRenderService.RenderToStringAsync("~/views/UmiStatus.cshtml", EndpointManager.All());
                    await httpContext.Response.WriteAsync(content);
                

            }
            else
            {
                await this.next(httpContext);
            }
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

        public static void AddUmi(this IServiceCollection serviceCollection)
        {
            // Add the embedded file provider
            var viewAssembly = typeof(UmiMiddleware).Assembly;
            var fileProvider = new EmbeddedFileProvider(viewAssembly);
            serviceCollection.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(fileProvider);
            });

            serviceCollection.AddSingleton<IViewRenderService, ViewRenderService>();


        }
    }


}

