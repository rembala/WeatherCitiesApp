using App.Helpers;
using System.Net.Http;
using Unity;

namespace App.Ioc {
    public class ContainerBuilder {
        public void RegisterTypes(UnityContainer container) {
            container.RegisterType<IMetasiteWeatherCitiesHelper, MetasiteWeatherCitiesHelper>();
            container.RegisterType<IMetasiteWeatherAuthorizationHeper, MetasiteAuthorizationHeper>();
            container.RegisterType<IMetasiteCitiesHelper, MetasiteCitiesHelper>();
            container.RegisterType<IMetasiteWeatherUrlForCityHelper, MetasiteWeatherUrlForCityHelper>();
            container.RegisterType<IMetasiteFileWriteHelper, MetasiteFileWriteHelper>();
            container.RegisterType<HttpClient, HttpClient>();
        }
    }
}
