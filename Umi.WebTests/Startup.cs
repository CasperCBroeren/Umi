using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umi.Core;
using Umi.Core.Authentication;

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
                    config.TestAsSuccessStatusCode = HttpStatusCode.BadRequest;
                    config.Category = "YouTube";
                });
            var endpoint2 = "https://www.googleapis.com/youtube/v2/activities".RegisterAsEndpoint(config =>
            { 
                config.Category = "YouTube";
                config.PreTest = (request) => {
                    request.Method = HttpMethod.Post;
                };
                config.PostTest = (response, result) =>
                {
                    result.Ok = response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Continue;
                };
            });

            var endpoint3 = "http://coincap.io/coins/".RegisterAsEndpoint(config =>
            { 
                config.Category = "Crypto";
            });
            var endpoint4 = "https://restcountries.eu/rest/v2/currency/eur".RegisterAsEndpoint();

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
                options.LocatorUrl = "/api/umi";
                options.Authentication = new BasicAuthentication("test", "test");
            });
            app.UseMvc();
        }
    }
}
