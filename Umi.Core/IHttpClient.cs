using System.Net.Http;
using System.Threading.Tasks;

namespace Umi.Core
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request); 
    }
}