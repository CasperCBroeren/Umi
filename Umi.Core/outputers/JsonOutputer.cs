using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Umi.Core.Outputer
{
    public class JsonOutputer : IOutputer
    {
        private UmiMiddlewareOptions options;

        public void SetOptions(UmiMiddlewareOptions options)
        {
            this.options = options;
        }

        public virtual async Task WriteOutput(HttpContext httpContext)
        {
            var allItems = await EndpointManager.All();
            httpContext.Response.ContentType = "application/json";
            // worst json serialiser ever
            await httpContext.Response.WriteAsync($@"{{ ""urls"": [{string.Join(",", allItems.Select(x => $@"{{ ""uri"": {StringOrNull(x.Uri)}, ""ok"": {x.TestResult.Ok.ToString().ToLower()}, ""tested"": {(int)x.TestResult.StatusCode}, ""testTo"": {(int)x.TestConfiguration.TestAsSuccessStatusCode}, ""category"": {StringOrNull(x.TestConfiguration.Category)}}}"))}]}}");
        }

        private string StringOrNull(string value) => !String.IsNullOrEmpty(value) ? string.Concat(@"""", value, @"""") : "null";
        private string StringOrNull(Uri value) => value != null ? string.Concat(@"""", value, @"""") : "null";
    }
}
