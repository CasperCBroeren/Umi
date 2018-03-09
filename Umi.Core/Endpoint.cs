using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Umi.Core
{
    public class Endpoint
    { 
        public Endpoint(string uriString, Action<EndpointConfiguration> configure) : this(new Uri(uriString), configure) { }        

        public Endpoint(Uri uri, Action<EndpointConfiguration> configure)
        {
            this.Uri = uri;
            this.TestConfiguration = new EndpointConfiguration()
            {
                TestAsSuccessStatusCode = HttpStatusCode.Accepted
            };
            if (configure != null)
            {
                configure(this.TestConfiguration);
            }
        }

        public Uri Uri { get; set; }

        public async Task DoTest()
        {

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(Uri);
                TestResult = new TestResult()
                {
                    Ok = response.StatusCode == HttpStatusCode.OK,
                    Response = response.Content.ToString()
                };
            }
        }
           

        public EndpointConfiguration TestConfiguration { get; set; }

        public TestResult TestResult
        {
            get;
            private set;
        }
    }
}
