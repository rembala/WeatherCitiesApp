using App.Helpers;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Infrastructure;

namespace Tests {
    public class MetasiteCitiesHelperTests : MetasiteBase {
        private MetasiteCitiesHelper _metasiteCitiesHelper;
        public MetasiteCitiesHelperTests(): base("[\"Tirana\",\"Yerevan\",\"Vienna\",\"Baku\",\"Minsk\",\"Brussels\",\"Sarajevo\",\"Sofia\",\"Zagreb\",\"Nicosia\",\"Prague\",\"Copenhagen\",\"Tallinn\",\"Helsinki\",\"Paris\",\"Tbilisi\",\"Berlin\",\"Athens\",\"Vilnius\"]") { }

        [SetUp]
        public void Setup() {

            var httpClient = new HttpClient(handlerMock.Object);
            _metasiteCitiesHelper = new MetasiteCitiesHelper(
                httpClient
            );
        }

        [Test]
        public async Task GetCitiesBasedOnMetasiteAuthorizationBearerAsync_SuccessfullyReturnsCities_ReturnsCities() {
            const string token = "bearerToken";

            var returnedAuthorizationToken = await _metasiteCitiesHelper
                .GetCitiesBasedOnMetasiteAuthorizationBearerAsync(token)
                .ConfigureAwait(false);

            Assert.IsNotNull(returnedAuthorizationToken);
        }
    }
}
