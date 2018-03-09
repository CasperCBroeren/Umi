using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umi.Core;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var endpoint1 = "https://www.googleapis.com/youtube/v3/activities".RegisterAsEndpoint(config =>
                {
                    config.TestAsSuccessStatusCode = HttpStatusCode.Forbidden;
                    config.Category = "YouTube";
                });
            var endpoint2 = "https://www.googleapis.com/youtube/v2/activities".RegisterAsEndpoint(config =>
            {
                config.TestAsSuccessStatusCode = HttpStatusCode.Forbidden;
                config.Category = "YouTube";
            });

            var endpoint3 = "http://coincap.io/coins/".RegisterAsEndpoint(config =>
            { 
                config.Category = "Crypto";
            });
            services.AddMvc();
            services.AddUmi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseUmi(options =>
            {
                options.LocatorUrl = "umi";
                options.ScaffoldWfc = false;
            });
            app.UseMvc();
        }
    }
}
