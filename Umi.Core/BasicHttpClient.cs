using System.Net.Http;
using System.Threading.Tasks;

namespace Umi.Core
{
    public class BasicHttpClient : IHttpClient
    {
        private HttpClient client;

        public BasicHttpClient()
        {
            this.client = new HttpClient();
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return this.client.SendAsync(request);
        }
    }
}
