using App.Infrastructure;
using App.Modals;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Helpers {
    public interface IMetasiteWeatherAuthorizationHeper {
        Task<AuthorizationResponse> GetAuthorizationResponseForUserNameAndPasswordAsync(string userName, string password);
    }

    public class MetasiteAuthorizationHeper : IMetasiteWeatherAuthorizationHeper {
        private readonly HttpClient _httpClient;

        public MetasiteAuthorizationHeper(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<AuthorizationResponse> GetAuthorizationResponseForUserNameAndPasswordAsync(
            string userName, string password) {
            using (EventLog eventLog = new EventLog("Application")) {
                eventLog.Source = "Application";
                try {
                    var authorizationRequest = new AuthorizationRequest { UserName = userName, Password = password };

                    var authorizationRequestSerialized = JsonConvert.SerializeObject(authorizationRequest);

                    var authorizationRequestContent = new StringContent(
                        authorizationRequestSerialized, Encoding.UTF8, "application/json");

                    var authorizeHttpReponse = await _httpClient
                        .PostAsync(MetasiteWeatherInfrastructure.AuthorizeUrl, authorizationRequestContent)
                        .ConfigureAwait(false);

                    var authorizedToken = await authorizeHttpReponse.Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);

                    var authorizationResponse = authorizedToken.GetDesirializedObject<AuthorizationResponse>();

                    eventLog.WriteEntry("Authorization is retrived", EventLogEntryType.Information, 101, 1);

                    return authorizationResponse;
                } catch (System.Exception ex) {
                    eventLog.WriteEntry(
                        $"Error occurred while retrieving authorization, exception message: {ex.Message}",
                        EventLogEntryType.Error, 101, 1);
                    throw ex;
                }
            }
        }
    }
}
