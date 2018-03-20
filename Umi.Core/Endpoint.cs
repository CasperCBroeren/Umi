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
                TestAsSuccessStatusCode = HttpStatusCode.OK,
                Category = string.Empty
            };

            configure?.Invoke(this.TestConfiguration);
        }

        public Uri Uri { get; set; }

        public async Task DoTest()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, Uri);
                    if (TestConfiguration.PreTest != null)
                    {
                        TestConfiguration.PreTest.Invoke(request);
                    }

                    var response = await client.SendAsync(request);
                    TestResult = new TestResult()
                    {
                        Ok = response.StatusCode == TestConfiguration.TestAsSuccessStatusCode,
                        StatusCode = response.StatusCode,
                        Response = response.Content.ToString()
                    };
                    if (TestConfiguration.PostTest != null)
                    {
                        TestConfiguration.PostTest.Invoke(response, TestResult);
                    }
                }
            }
            catch (Exception exc)
            {
                TestResult = new TestResult()
                {
                    Ok = false,
                    StatusCode = HttpStatusCode.SeeOther,
                    Response = exc.Message
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
