using App.Helpers;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tests.Infrastructure;

namespace Tests {
    public class MetasiteAuthorizationHeperTests : MetasiteBase {
        private MetasiteAuthorizationHeper _metasiteAuthorizationHeper;

        public MetasiteAuthorizationHeperTests() : base(
            "{\"bearer\":\"ac8cc796-e1f0-48d2-a8d0-93e2cbb9d415\"}") { 
        }

        [SetUp]
        public void Setup() {
            var httpClient = new HttpClient(handlerMock.Object);
            _metasiteAuthorizationHeper = new MetasiteAuthorizationHeper(
                httpClient
            );
        }

        [Test]
        public async Task GetAuthorizationResponseForUserNameAndPasswordAsync_AuthorizationSuccessResponse_ReturnsAuthorizationToken() {
            const string userName = "test";
            const string password = "test123";

            var returnedAuthorizationToken = await _metasiteAuthorizationHeper
                .GetAuthorizationResponseForUserNameAndPasswordAsync(userName, password)
                .ConfigureAwait(false);

            Assert.AreEqual("ac8cc796-e1f0-48d2-a8d0-93e2cbb9d415", returnedAuthorizationToken.Bearer);
        }
    }
}
