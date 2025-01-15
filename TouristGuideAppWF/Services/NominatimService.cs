using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TouristGuideAppWF.Services
{
    // This service handles interactions with the Nominatim API to fetch geographical coordinates.
    public class NominatimService : BaseService
    {
        // Constructor initializes the service and sets the base address for the HttpClient.
        public NominatimService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            httpClient.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
        }

        /// <summary>
        /// Retrieves the latitude and longitude of a given city name using the Nominatim API.
        /// </summary>
        /// <param name="cityName">The name of the city to fetch coordinates for.</param>
        /// <returns>A tuple containing the latitude and longitude of the city.</returns>
        /// <exception cref="Exception">Thrown if the city is not found or if there's an issue with the API request.</exception>
        public async Task<(double latitude, double longitude)> GetCoordinatesAsync(string cityName)
        {
            // Build the API request URL.
            string url = $"search?q={Uri.EscapeDataString(cityName)}&format=json&limit=1";

            try
            {
                // Make the API call and ensure the response is successful.
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Parse the response JSON content.
                var responseBody = await response.Content.ReadAsStringAsync();
                using JsonDocument jsonData = JsonDocument.Parse(responseBody);

                // Validate the response structure and data.
                if (jsonData.RootElement.ValueKind == JsonValueKind.Array && jsonData.RootElement.GetArrayLength() == 0)
                    throw new Exception($"City '{cityName}' not found.");

                // Extract latitude and longitude from the first result.
                var firstResult = jsonData.RootElement[0];
                double latitude = double.Parse(firstResult.GetProperty("lat").GetString());
                double longitude = double.Parse(firstResult.GetProperty("lon").GetString());

                return (latitude, longitude);
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP-specific errors.
                throw new Exception($"HTTP error while fetching coordinates: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle general errors.
                throw new Exception($"Unexpected error: {ex.Message}", ex);
            }
        }
    }
}
