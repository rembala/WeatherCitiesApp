using App.Modals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.Helpers {
    public interface IMetasiteWeatherCitiesHelper {
        Task<List<CityWeather>> GetCitiesWeatherAsync(List<string> cities, string token);
        string GetConcatenatedCitiesWeathers(List<CityWeather> citiesWeather);
    }

    public class MetasiteWeatherCitiesHelper: IMetasiteWeatherCitiesHelper {
        private readonly IMetasiteWeatherUrlForCityHelper _metasiteWeatherUrlForCityHelper;
        private readonly HttpClient _httpClient;

        public MetasiteWeatherCitiesHelper(
            IMetasiteWeatherUrlForCityHelper metasiteWeatherUrlForCityHelper,
            HttpClient httpClient) {
            _metasiteWeatherUrlForCityHelper = metasiteWeatherUrlForCityHelper;
            _httpClient = httpClient;
        }

        public async Task<List<CityWeather>> GetCitiesWeatherAsync(List<string> cities, string token) {
            using (EventLog eventLog = new EventLog("Application")) {
                eventLog.Source = "Application";

                try {
                    var citiesWeather = new List<CityWeather>();

                    foreach (var city in cities) {
                        var weatherCityUrl = _metasiteWeatherUrlForCityHelper.GetMetasiteWeatherUrlForCity(city);

                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                        var cityWeatherResponse = await _httpClient
                            .GetStringAsync(weatherCityUrl)
                            .ConfigureAwait(false);

                        var cityWeather = cityWeatherResponse.GetDesirializedObject<CityWeather>();

                        citiesWeather.Add(cityWeather);
                    }

                    if (citiesWeather.Count > 0) {
                        eventLog.WriteEntry("Sucesfully retrieved city weathers", EventLogEntryType.Information, 101, 1);
                    }

                    return citiesWeather;
                } catch (Exception ex) { 
                    eventLog.WriteEntry(
                        $"Error occurred while retrieving cities weather, exception message: {ex.Message}",
                        EventLogEntryType.Error, 101, 1);
                    throw ex;
                }
            }
        }

        public string GetConcatenatedCitiesWeathers(List<CityWeather> citiesWeather) {
            var cityWeatherBuilder = new StringBuilder();

            foreach (var cityWeather in citiesWeather) {
                var weather = string.Format(
                    "City - {0}, Temperature - {1}, Precipitation - {2}, Weather - {3}",
                    cityWeather.City, cityWeather.Temperature, cityWeather.Precipitation, cityWeather.Weather);
                cityWeatherBuilder.Append(weather + Environment.NewLine);
            }

            return cityWeatherBuilder.ToString();
        }
    }
}
