using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using Umi.Core.Outputer;

namespace Umi.Core
{
    public static class UmiMiddlewareExtentions
    {
        public static void AddUmi(this IServiceCollection serviceCollection)
        {
            
            var viewAssembly = typeof(UmiMiddleware).Assembly;
            var fileProvider = new EmbeddedFileProvider(viewAssembly);
            serviceCollection.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(fileProvider);
            });

            serviceCollection.AddSingleton<IViewRenderService, ViewRenderService>();
            serviceCollection.AddSingleton<AssetOutputer>();
            serviceCollection.AddSingleton<HtmlOutputer>();
            serviceCollection.AddSingleton<JsonOutputer>(); 

        }

        public static IApplicationBuilder UseUmi(this IApplicationBuilder builder, Action<UmiMiddlewareOptions> configureOptions = null)
        {
            var options = new UmiMiddlewareOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }

            return builder.UseMiddleware<UmiMiddleware>(options);
        }

   
    }


}

