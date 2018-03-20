using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Umi.Core;
using Umi.Core.Authentication;
using Umi.Core.Outputer;

namespace Umi.Tests
{

    [TestClass]
    public class UmiMiddlewareAuthenticationTests
    {
        private Mock<HtmlOutputer> htmlOutputer;
        private Mock<AssetOutputer> assetOutputer;
        private UmiMiddleware middleWare;
        private Mock<JsonOutputer> jsonOutputer;

        [TestInitialize]
        public void Setup()
        {
           
            jsonOutputer = new Mock<JsonOutputer>();
            jsonOutputer.Setup(x => x.WriteOutput(It.IsAny<HttpContext>())).Returns(Task.CompletedTask).Verifiable();
            htmlOutputer = new Mock<HtmlOutputer>(null);
            htmlOutputer.Setup(x => x.WriteOutput(It.IsAny<HttpContext>())).Returns(Task.CompletedTask).Verifiable();
            assetOutputer = new Mock<AssetOutputer>();
            assetOutputer.Setup(x => x.WriteOutput(It.IsAny<HttpContext>())).Returns(Task.CompletedTask).Verifiable();
            middleWare = new UmiMiddleware(null, new UmiMiddlewareOptions() { LocatorUrl = "/test", Authentication = new BasicAuthentication("test", "test") },
               jsonOutputer.Object,
               htmlOutputer.Object,
                assetOutputer.Object
                );
        }

        [TestMethod]
        public async Task NotAuthorized_GetHeaders()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";
            httpContext.Request.Headers.Add("accept", "application/json");

            await middleWare.Invoke(httpContext);
            jsonOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
            htmlOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
            assetOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
            Assert.IsTrue(httpContext.Response.Headers.ContainsKey("WWW-Authenticate"));
            Assert.IsTrue(httpContext.Response.StatusCode.Equals(401));
        }

        [TestMethod]
        public async Task Authorized_GetJson()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Authorization", "Basic dGVzdDp0ZXN0");
            httpContext.Request.Path = "/test";
            httpContext.Request.Headers.Add("accept", "application/json");

            await middleWare.Invoke(httpContext);
            jsonOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Once);
            htmlOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
            assetOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
   
        }

        [TestMethod]
        public async Task NotCorrectAuthorized_Forbidden()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Authorization", "basic foo");
            httpContext.Request.Path = "/test";
            httpContext.Request.Headers.Add("accept", "application/json");

            await middleWare.Invoke(httpContext);
            jsonOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
            htmlOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
            assetOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
            Assert.IsTrue(httpContext.Response.StatusCode.Equals(403));
        }
    }
}
