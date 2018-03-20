using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Umi.Core.Outputer
{
    public interface IOutputer
    {
        Task WriteOutput(HttpContext httpContext);
        void SetOptions(UmiMiddlewareOptions options);
    }
}