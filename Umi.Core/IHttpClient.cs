using System.Net.Http;

namespace Umi.Core
{
    public interface IHttpClient
    {
        HttpResponseMessage DoRequest(string url);
    }
}