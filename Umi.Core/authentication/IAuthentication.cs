using Microsoft.AspNetCore.Http;

namespace Umi.Core.Authentication
{
    public interface IAuthentication
    {
        bool TriesToAuthenticate(HttpRequest request);
        bool IsAuthenticated(HttpRequest request);
    }
}