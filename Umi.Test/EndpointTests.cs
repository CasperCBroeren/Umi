using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var result = await EndpointManager.All();
            Assert.AreEqual(2, result.Count);
        }
    }
}
