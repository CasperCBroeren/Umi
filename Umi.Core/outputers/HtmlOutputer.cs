using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Umi.Core.Outputer
{
    public class HtmlOutputer : IOutputer
    {
        private readonly IViewRenderService viewRenderService;
        private UmiMiddlewareOptions options;

        public void SetOptions(UmiMiddlewareOptions options)
        {
            this.options = options;
        }

        public HtmlOutputer(IViewRenderService viewRenderService)
        {
            this.viewRenderService = viewRenderService;
        } 

        public virtual async Task WriteOutput(HttpContext httpContext)
        {
            var allItems = await EndpointManager.All();
            var content = await this.viewRenderService.RenderToStringAsync("~/views/UmiStatus.cshtml", allItems, options.LocatorUrl);
            await httpContext.Response.WriteAsync(content);
        }
    }
}