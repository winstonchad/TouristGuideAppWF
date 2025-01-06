using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TouristGuideAppWF.Services
{
    public class NominatimService
    {
        private readonly HttpClient _httpClient;

        public NominatimService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "TouristGuideApp/1.0 (your_email@example.com)");
        }

        public async Task<(double latitude, double longitude)> GetCoordinatesAsync(string cityName)
        {
            try
            {
                string url = $"search?q={Uri.EscapeDataString(cityName)}&format=json&limit=1";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                using JsonDocument jsonData = JsonDocument.Parse(responseBody);

                if (jsonData.RootElement.ValueKind == JsonValueKind.Array && jsonData.RootElement.GetArrayLength() == 0)
                {
                    throw new Exception($"City '{cityName}' not found.");
                }

                var firstResult = jsonData.RootElement[0];
                double latitude = double.Parse(firstResult.GetProperty("lat").GetString());
                double longitude = double.Parse(firstResult.GetProperty("lon").GetString());

                return (latitude, longitude);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"HTTP error while getting coordinates: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}", ex);
            }
        }

    }
}
