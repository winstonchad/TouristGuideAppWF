using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TouristGuideAppWF.Services
{
    public class GeminiService : BaseService
    {
        private readonly string _googleAiApiKey;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
            // Retrieve the API key from configuration and ensure it is valid
            _googleAiApiKey = _configuration["GoogleAi:ApiKey"]
                ?? throw new InvalidOperationException("Google Gemini API key is missing in appsettings.json");
        }

        /// <summary>
        /// Retrieves tourist attractions for a given city using the Google Gemini API.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        /// <returns>A string containing the tourist attractions or an error message.</returns>
        public async Task<string> GetTouristAttractionsAsync(string cityName)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(cityName))
                return "City name cannot be empty.";

            // Define the URL for the Gemini API
            string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

            // Prepare the request body
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = $"List 5 tourist attractions in {cityName} with a short description." }
                        }
                    }
                }
            };

            // Serialize the request body to JSON
            string jsonBody = JsonSerializer.Serialize(requestBody);

            // Create the HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            // Add required headers
            request.Headers.Add("x-goog-api-key", _googleAiApiKey);
            request.Headers.Add("Accept", "application/json");

            try
            {
                // Use the base service method to send the request
                string responseBody = await SendRequestAsync(request);

                // Parse the response JSON
                using JsonDocument jsonResponse = JsonDocument.Parse(responseBody);

                // Extract and return tourist attractions if available
                if (jsonResponse.RootElement.TryGetProperty("candidates", out var candidates) &&
                    candidates.GetArrayLength() > 0)
                {
                    return candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                }

                return "No tourist attractions found.";
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return $"An error occurred while retrieving tourist attractions: {ex.Message}";
            }
        }
    }
}
