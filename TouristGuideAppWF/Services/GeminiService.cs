using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TouristGuideAppWF.Services
{
    internal class GeminiService
    {
        public readonly HttpClient httpClient;
        private readonly string googleAiApiKey;

        public GeminiService(IConfiguration configuration)
        {
            httpClient = new HttpClient();
            googleAiApiKey = configuration["GoogleAi:ApiKey"];

            if (string.IsNullOrEmpty(googleAiApiKey))
            {
                throw new InvalidOperationException("Weather API key is missing in appsettings.json");
            }
        }

        public async Task<string> GetTouristAttractionsAsync(string cityName)
        {
            string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    // Устанавливаем заголовки
                    request.Headers.Add("x-goog-api-key", googleAiApiKey);
                    request.Headers.Add("Accept", "application/json");

                    // Формируем тело запроса
                    var requestBody = new
                    {
                        contents = new[] {
                            new {
                                 parts = new[] {
                                          new { text = $"List 5 tourist attractions in {cityName} with a short description for each. Each description should be 1-2 sentences." }
                                 }
                             }
                         }
                    };

                    string jsonBody = JsonSerializer.Serialize(requestBody);
                    request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"HTTP error: {response.StatusCode}, {errorResponse}");
                    }

                    string responseBody = await response.Content.ReadAsStringAsync();
                    using JsonDocument jsonResponse = JsonDocument.Parse(responseBody);

                    if (jsonResponse.RootElement.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                    {
                        var candidate = candidates[0];
                        string result = candidate.GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

                        return result;
                    }
                    else
                    {

                        return "No tourist attractions found in Gemini response.";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return $"HTTP error while getting response from Gemini: {ex.Message}, Inner Exception: {ex.InnerException?.Message}";
            }
            catch (Exception ex)
            {
                return $"Error in GetTouristAttractionsAsync: {ex.Message}, Stack Trace: {ex.StackTrace}";
            }
        }
    }
}
