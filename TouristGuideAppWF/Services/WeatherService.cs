using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TouristGuideAppWF.Services
{
    public class WeatherService
    {
        public readonly HttpClient httpClient;
        public readonly string weatherApiKey;

        public WeatherService(IConfiguration configuration) 
        { 
            httpClient = new HttpClient();
            weatherApiKey = configuration["WeatherApi:ApiKey"];

            if (string.IsNullOrEmpty(weatherApiKey))
            {
                throw new InvalidOperationException("Weather API key is missing in appsettings.json");
            }
        }


        public async Task<string> GetWeatherAsync(string cityName, double lat, double lon)
        {
            

            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={weatherApiKey}&units=metric";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                using JsonDocument json = JsonDocument.Parse(responseBody);

                string weatherDescription = json.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();
                double temperature = json.RootElement.GetProperty("main").GetProperty("temp").GetDouble();

                return $"The weather in {cityName} is {weatherDescription} and the temperature is {temperature}°C.";
            }

            catch (HttpRequestException ex)
            {
                return $"HTTP error while fetching weather: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Unexpected error: {ex.Message}";
            }
        }

    }
}
