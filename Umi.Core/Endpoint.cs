using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Umi.Core
{
    public class Endpoint
    { 
        public Endpoint(string uriString) : this(new Uri(uriString)) { }        

        public Endpoint(Uri uri)
        {
            this.Uri = uri;
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

        public TestResult TestResult
        {
            get;
            private set;
        }
    }
}
