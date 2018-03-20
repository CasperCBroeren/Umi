using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Umi.Core;

namespace Umi.Test
{
    [TestClass]
    public class EndpointTests
    {
        [TestMethod]
        public async Task RegisterTwoEndpoints()
        {
            var httpApiEndpoint = "http://someapi.atsomedomain.com/v1".RegisterAsEndpoint();
            var uriApiEndpoint = new Uri("http://www.google.com").RegisterAsEndpoint();
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.Accepted)));
            var result = await EndpointManager.All(httpClientMock.Object);
            Assert.AreEqual(2, result.Count);
        }
    }
}
