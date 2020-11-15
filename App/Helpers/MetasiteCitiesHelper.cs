using App.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace App.Helpers {
    public interface IMetasiteCitiesHelper {
        Task<List<string>> GetCitiesBasedOnMetasiteAuthorizationBearerAsync(string bearer);
    }

    public class MetasiteCitiesHelper : IMetasiteCitiesHelper {
        private readonly HttpClient _httpClient;

        public MetasiteCitiesHelper(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetCitiesBasedOnMetasiteAuthorizationBearerAsync(string bearer) {
            using (EventLog eventLog = new EventLog("Application")) {
                eventLog.Source = "Application";
                try {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", bearer);

                    var citiesResponse = await _httpClient
                        .GetStringAsync(MetasiteWeatherInfrastructure.CitiesUrl)
                        .ConfigureAwait(false);

                    var cities = citiesResponse.GetDesirializedObject<string[]>();
                    if (cities.Length > 0) {
                        eventLog.WriteEntry("Sucesfully retrieved cities", EventLogEntryType.Information, 101, 1);
                    }

                    return cities.ToList();
                } catch (System.Exception ex) {
                    eventLog.WriteEntry(
                        $"Error occurred while retrieving cities, exception message: {ex.Message}",
                        EventLogEntryType.Error, 101, 1);
                    throw ex;
                }
            }
        }
    }
}
