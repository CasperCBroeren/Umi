using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Umi.Core.Outputer
{
    public class HtmlOutputer : IOutputer
    {
        private readonly IViewRenderService viewRenderService;
        private readonly IHttpClient httpClient;
        private UmiMiddlewareOptions options;

        public void SetOptions(UmiMiddlewareOptions options)
        {
            this.options = options;
        }

        public HtmlOutputer(IViewRenderService viewRenderService,
                            IHttpClient httpClient)
        {
            this.viewRenderService = viewRenderService;
            this.httpClient = httpClient;
        } 

        public virtual async Task WriteOutput(HttpContext httpContext)
        {
            var allItems = await EndpointManager.All(this.httpClient);
            var content = await this.viewRenderService.RenderToStringAsync("~/views/UmiStatus.cshtml", allItems, options.LocatorUrl);
            await httpContext.Response.WriteAsync(content);
        }
    }
}