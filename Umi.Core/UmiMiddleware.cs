using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
               
                if (IsJsonRequest(httpContext))
                {
                    var allItems = await EndpointManager.All();
                    httpContext.Response.ContentType = "application/json";
                    // worst json serialiser ever
                    await httpContext.Response.WriteAsync($@"{{ ""urls"": [{string.Join(",", allItems.Select(x=> $@"{{ ""uri"": ""{x.Uri}"", ""ok"": {x.TestResult.Ok.ToString().ToLower()}}}"))}]}}"); 
                }
                else if (url.StartsWithSegments(options.LocatorAssetUrl))
                {
                    var assetName = url.ToString().Replace($"{options.LocatorAssetUrl}/", string.Empty);

                    using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Umi.Core.assets.{assetName}"))
                    {
                        await assetStream.CopyToAsync(httpContext.Response.Body);
                    }


                }
                else
                {
                    var allItems = await EndpointManager.All();
                    var content = await this.viewRenderService.RenderToStringAsync("~/views/UmiStatus.cshtml", allItems);
                    await httpContext.Response.WriteAsync(content);
                }

            }
            else
            {
                await this.next(httpContext);
            }
        }

        private static bool IsJsonRequest(HttpContext httpContext)
        {
            return httpContext.Request.Headers["accept"] == "application/json" ||
                httpContext.Request.Headers["accept"] == "text/javascript";

        }
    }

    public static class UmiMiddlewareExtentions
    {

        public static IApplicationBuilder UseUmi(this IApplicationBuilder builder, Action<UmiMiddlewareOptions> configureOptions = null)
        {
            var options = new UmiMiddlewareOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }

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

