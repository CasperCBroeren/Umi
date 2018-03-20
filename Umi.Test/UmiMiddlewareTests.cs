using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Umi.Core;
using Umi.Core.Outputer;

namespace Umi.Tests
{

    [TestClass]
    public class UmiMiddlewareTests
    {
        private Mock<HtmlOutputer> htmlOutputer;
        private Mock<AssetOutputer> assetOutputer;
        private UmiMiddleware middleWare;
        private Mock<JsonOutputer> jsonOutputer;

        [TestInitialize]
        public void Setup()
        {
            var httpClientMock = new Mock<IHttpClient>();
            jsonOutputer = new Moq.Mock<JsonOutputer>(httpClientMock.Object);
            jsonOutputer.Setup(x => x.WriteOutput(It.IsAny<HttpContext>())).Returns(Task.CompletedTask).Verifiable();
            htmlOutputer = new Moq.Mock<HtmlOutputer>( null, httpClientMock.Object);
            htmlOutputer.Setup(x => x.WriteOutput(It.IsAny<HttpContext>())).Returns(Task.CompletedTask).Verifiable();
            assetOutputer = new Moq.Mock<AssetOutputer>();
            assetOutputer.Setup(x => x.WriteOutput(It.IsAny<HttpContext>())).Returns(Task.CompletedTask).Verifiable();
            middleWare = new UmiMiddleware(null, new UmiMiddlewareOptions() { LocatorUrl = "/test" },
               jsonOutputer.Object,
               htmlOutputer.Object,
                assetOutputer.Object
                );
        }

        [TestMethod]
        public async Task OutputJson_Correct()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";
            httpContext.Request.Headers.Add("accept", "application/json");

            await middleWare.Invoke(httpContext);
            jsonOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Once);
        }

        [TestMethod]
        public async Task OutputJson_InCorrectHeader()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";
            httpContext.Request.Headers.Add("contentAccept", "application/json");

            await middleWare.Invoke(httpContext);
            jsonOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
        }

        [TestMethod]
        public async Task OutputHtml_Correct()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";
            httpContext.Request.Headers.Add("contentAccept", "application/json");

            await middleWare.Invoke(httpContext);
            htmlOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Once);
        }

        [TestMethod]
        public async Task OutputAsset_Correct()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test/assets/foo";
            httpContext.Request.Headers.Add("contentAccept", "application/json");

            await middleWare.Invoke(httpContext);
            assetOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Once);
        }

        [TestMethod]
        public async Task OutputJson_InCorrectUrl()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/never";
            httpContext.Request.Headers.Add("accept", "application/json");

            await middleWare.Invoke(httpContext);
            jsonOutputer.Verify(t => t.WriteOutput((It.IsAny<HttpContext>())), Times.Never);
        }
    }
}
