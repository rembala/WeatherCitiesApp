using App.Helpers;
using App.Ioc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity;

namespace App {
    class Program {
        static async Task Main(string[] args) {
            var container = new UnityContainer();
            container.RegisterType<MetasiteWeatherAPI, MetasiteWeatherAPI>();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterTypes(container);

            await DisplayAndSaveMetasiteCitiesWeatherAsync(container)
                .ConfigureAwait(false);
        }

        private static async Task DisplayAndSaveMetasiteCitiesWeatherAsync(UnityContainer container) {
            while (true) {
                await Task.Run(async () => {
                    using (EventLog eventLog = new EventLog("Application")) {
                        try {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Meatasite city weathers application is started", EventLogEntryType.Information, 101, 1);

                            var metasiteApi = container.Resolve<MetasiteWeatherAPI>();

                            var citiesWeather = await metasiteApi
                                .GetCitiesWeatherAsync()
                                .ConfigureAwait(false);

                            var metasiteWeatherCitiesHelper = container.Resolve<IMetasiteWeatherCitiesHelper>();

                            var concatenatedCitiesWeather = metasiteWeatherCitiesHelper
                                .GetConcatenatedCitiesWeathers(citiesWeather);

                            Console.WriteLine("Cities weathers:");

                            Console.WriteLine(concatenatedCitiesWeather);

                            var metasiteFileWriteHelper = container.Resolve<IMetasiteFileWriteHelper>();

                            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                            await metasiteFileWriteHelper
                                .WriteTextAsync(desktopPath, concatenatedCitiesWeather)
                                .ConfigureAwait(false);

                            var timeSpanDelay = new TimeSpan(0, 0, 30);

                            await Task.Delay(timeSpanDelay);

                        } catch (Exception ex) {
                            eventLog.WriteEntry(
                                $"Exception occurred during weather display and saving, message: {ex.Message}",
                                EventLogEntryType.Error, 101, 1);
                            throw ex;
                        }
                    }
                });
            }
        }
    }
}
