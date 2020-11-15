using App.Helpers;
using App.Modals;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App {
    public class MetasiteWeatherAPI {
        private readonly IMetasiteWeatherAuthorizationHeper _metasiteWeatherAuthorizationHeper;
        private readonly IMetasiteWeatherCitiesHelper _metasiteWeatherCitiesHelper;
        private readonly IMetasiteCitiesHelper _metasiteCitiesHelper;

        public MetasiteWeatherAPI(
            IMetasiteWeatherAuthorizationHeper metasiteWeatherAuthorizationHeper,
            IMetasiteCitiesHelper metasiteCitiesHelper,
            IMetasiteWeatherCitiesHelper metasiteWeatherCitiesHelper
            ) {
            _metasiteWeatherAuthorizationHeper = metasiteWeatherAuthorizationHeper;
            _metasiteCitiesHelper = metasiteCitiesHelper;
            _metasiteWeatherCitiesHelper = metasiteWeatherCitiesHelper;
        }

        public async Task<List<CityWeather>> GetCitiesWeatherAsync() {
            var authorizationResponse = await _metasiteWeatherAuthorizationHeper
                .GetAuthorizationResponseForUserNameAndPasswordAsync("meta", "site")
                .ConfigureAwait(false);

            if (authorizationResponse == null) {
                throw new Exception("Authorization required");
            }

            var cities = await _metasiteCitiesHelper
                .GetCitiesBasedOnMetasiteAuthorizationBearerAsync(authorizationResponse.Bearer)
                .ConfigureAwait(false);

            if (cities == null) {
                throw new Exception("Cities are empty");
            }

            var citiesWeather = await _metasiteWeatherCitiesHelper
                .GetCitiesWeatherAsync(cities, authorizationResponse.Bearer)
                .ConfigureAwait(false);

            if (citiesWeather == null) {
                throw new Exception("city weathers are empty");
            }

            return citiesWeather;
        }
    }
}
