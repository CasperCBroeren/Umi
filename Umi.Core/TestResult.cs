using System.Net;

namespace Umi.Core
{
    public struct TestResult
    {
        public bool Ok { get; set; }
        public string Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
