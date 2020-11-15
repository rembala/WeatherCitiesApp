using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Infrastructure {
    public abstract class MetasiteBase {
        protected Mock<HttpMessageHandler> handlerMock;

        public MetasiteBase(string content) {
            handlerMock = new Mock<HttpMessageHandler>();
            SetupHttpMessageHandler(content);
        }

        private void SetupHttpMessageHandler(string content) {
            var response = new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content),
            };

            handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        }
    }
}
