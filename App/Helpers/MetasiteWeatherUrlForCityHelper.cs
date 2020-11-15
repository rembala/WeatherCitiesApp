namespace App.Helpers {
    public interface IMetasiteWeatherUrlForCityHelper {
        string GetMetasiteWeatherUrlForCity(string city);
    }

    public class MetasiteWeatherUrlForCityHelper : IMetasiteWeatherUrlForCityHelper {
        public string GetMetasiteWeatherUrlForCity(string city) =>
            $"https://metasite-weather-api.herokuapp.com/api/Weather/{city}";
    }
}
