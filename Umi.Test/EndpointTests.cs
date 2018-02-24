using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Umi.Core;

namespace Umi.Test
{
    [TestClass]
    public class EndpointTests
    {
        [TestMethod]
        public void RegisterTwoEndpoints()
        {
            var httpApiEndpoint = "http://someapi.atsomedomain.com/v1".RegisterAsEndpoint();
            var uriApiEndpoint = new Uri("http://www.google.com").RegisterAsEndpoint();
            Assert.AreEqual(2, EndpointManager.All().Count);
        }
    }
}
