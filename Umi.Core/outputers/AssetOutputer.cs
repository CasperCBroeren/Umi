using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Umi.Core.Outputer
{
    public class AssetOutputer : IOutputer
    { 
        private UmiMiddlewareOptions options;

        public void SetOptions(UmiMiddlewareOptions options)
        {
            this.options = options;
        }

        public virtual async Task WriteOutput(HttpContext httpContext)
        {
            var url = httpContext.Request.Path;
            var assetName = url.ToString().Replace($"{options.LocatorAssetUrl}/", string.Empty);

            using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Umi.Core.assets.{assetName}"))
            {
                await assetStream.CopyToAsync(httpContext.Response.Body);
            }
        }
    }
}