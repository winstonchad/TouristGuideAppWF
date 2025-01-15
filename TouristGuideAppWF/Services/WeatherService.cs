using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TouristGuideAppWF.Services
{
    /// <summary>
    /// Service for interacting with the OpenWeatherMap API to fetch weather data.
    /// </summary>
    public class WeatherService : BaseService
    {
        // The API key used for authentication with the OpenWeatherMap API.
        public readonly string weatherApiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests.</param>
        /// <param name="configuration">The configuration containing the API key.</param>
        /// <exception cref="InvalidOperationException">Thrown if the API key is missing.</exception>
        public WeatherService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
            weatherApiKey = configuration["WeatherApi:ApiKey"]
                ?? throw new InvalidOperationException("Weather API key is missing in appsettings.json");
        }

        /// <summary>
        /// Retrieves the current weather information for a given city.
        /// </summary>
        /// <param name="cityName">The name of the city.</param>
        /// <param name="lat">The latitude of the city.</param>
        /// <param name="lon">The longitude of the city.</param>
        /// <returns>A formatted string describing the weather and temperature.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the API request or response parsing.</exception>
        public async Task<string> GetWeatherAsync(string cityName, double lat, double lon)
        {
            // Construct the API request URL with the provided parameters.
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={weatherApiKey}&units=metric";

            // Validate the input city name.
            if (string.IsNullOrEmpty(cityName))
                return "City name is missing.";

            // Send the request using the BaseService method for consistency.
            string responseBody = await SendRequestAsync(url);

            // Parse the JSON response to extract weather details.
            using JsonDocument json = JsonDocument.Parse(responseBody);

            // Extract specific fields from the JSON response.
            string weatherDescription = json.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();
            double temperature = json.RootElement.GetProperty("main").GetProperty("temp").GetDouble();

            // Return the formatted weather information.
            return $"The weather in {cityName} is {weatherDescription} and the temperature is {temperature}°C.";
        }
    }
}
