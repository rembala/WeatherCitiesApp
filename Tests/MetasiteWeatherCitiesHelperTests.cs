using App.Helpers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Infrastructure;

namespace MetasiteWeatherCitiesHelperTests {
    public class MetasiteWeatherCitiesHelperTests : MetasiteBase {
        private MetasiteWeatherCitiesHelper _metasiteWeatherCitiesHelper;
        private Mock<IMetasiteWeatherUrlForCityHelper> _metasiteWeatherUrlForCityHelperMock;

        public MetasiteWeatherCitiesHelperTests():
            base("{\"city\":\"Tirana\",\"temperature\":-11.0,\"precipitation\":14,\"weather\":\"Thunderstorm\"}") { }

        [SetUp]
        public void Setup() {
            var httpClient = new HttpClient(handlerMock.Object);
            _metasiteWeatherUrlForCityHelperMock = new Mock<IMetasiteWeatherUrlForCityHelper>();
            _metasiteWeatherCitiesHelper = new MetasiteWeatherCitiesHelper(
                _metasiteWeatherUrlForCityHelperMock.Object,
                httpClient
            );
        }

        [Test]
        public async Task GetCitiesWeatherAsync_CityWeatherSuccessResponse_ReturnsCity() {
            var cities = new List<string> { "Tirana"};
            const string token = "ad267fb2-d0b8-4ec7-9050-4a6b14c36ea5";
            _metasiteWeatherUrlForCityHelperMock
                .Setup(h => h.GetMetasiteWeatherUrlForCity(It.IsAny<string>()))
                .Returns((string value) => $"https://metasite-weather-api.herokuapp.com/api/Weather/{value}");

            var returnedCities = await _metasiteWeatherCitiesHelper
                .GetCitiesWeatherAsync(cities, token)
                .ConfigureAwait(false);

            _metasiteWeatherUrlForCityHelperMock
                .Verify(h => h.GetMetasiteWeatherUrlForCity(It.Is<string>(city => cities.Contains(city))), Times.Once);
            Assert.IsTrue(returnedCities.Any(returnedCity => returnedCity.City == "Tirana"));
        }
    }
}