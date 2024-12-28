using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GenerativeAI;
using GenerativeAI.Models;
using System.Text;

namespace TouristGuideAppWF
{
    public class NominatimService
    {
        private readonly HttpClient _httpClient;

        public NominatimService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName/1.0 (your_email@example.com)");
        }
    }

    public partial class Form1 : Form
    {
        private readonly IConfigurationRoot _configuration;
        private readonly HttpClient _httpClient;

        public Form1(IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();
            _configuration = LoadConfiguration();
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "TouristGuideApp 1.0 (amirkass84@gmail.com) ");
        }

        private IConfigurationRoot LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        public async Task<(double latitude, double longitude)> GetCoordinatesAsync(string cityName)
        {
            try
            {
                string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(cityName)}&format=json&limit=1";

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


        private async void SearchBtn_Click(object sender, EventArgs e)
        {
            string cityName = CityNameInput.Text.Trim();

            if (string.IsNullOrEmpty(cityName))
            {
                MessageBox.Show("Please enter a valid city name.");
                return;
            }

            try
            {
                var (latitude, longitude) = await GetCoordinatesAsync(cityName);

                // Вызов методов для получения информации
                string weatherInfo = await GetWeatherAsync(cityName, latitude, longitude);
                string touristInfo = await GetTouristAttractionsAsync(cityName);

                // Вывод результатов в текстовое поле
                ResultTextBox.Text = $"Weather Info:\n{weatherInfo}\n\nTourist Attractions:\n{touristInfo}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}, {ex.StackTrace}");
            }
        }

        public async Task<string> GetWeatherAsync(string cityName, double lat, double lon)
        {
            string weatherApiKey = _configuration["WeatherApi:ApiKey"];

            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={weatherApiKey}&units=metric";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                using JsonDocument json = JsonDocument.Parse(responseBody);

                string weatherDescription = json.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();
                double temperature = json.RootElement.GetProperty("main").GetProperty("temp").GetDouble();

                return $"The weather in {cityName} is {weatherDescription} and the temperature is {temperature}°C.";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        public async Task<string> GetTouristAttractionsAsync(string cityName)
        {
            string googleAiApiKey = _configuration["GoogleAi:ApiKey"];
            if (string.IsNullOrWhiteSpace(googleAiApiKey))
            {
                throw new InvalidOperationException("Google Gemini API Key is missing!");
            }

            try
            {
                string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

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

                    using HttpClient httpClient = new HttpClient();
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









        private void textBox1_TextChanged(object sender, EventArgs e){}

        private void ResultTextBox_TextChanged(object sender, EventArgs e) {}
    }
}


//public async Task<string> GetTouristInfoAsyncGPT(string cityName)
//{
//    string openAiApiKey = _configuration["OpenAI:ApiKey"];

//    if (string.IsNullOrEmpty(openAiApiKey))
//    {
//        throw new InvalidOperationException("OpenAI API key is missing in appsettings.json");
//    }
//    try
//    {
//        ChatClient client = new(model: "gpt-3.5-turbo", apiKey: openAiApiKey);

//        string prompt = $"List 5 tourist attractions in {cityName} with a short description for each. Each description should be 1-2 sentences.";

//        ChatCompletion completion = client.CompleteChat(prompt);
//        if (completion?.Content != null && completion.Content.Count > 0)
//        {
//            return completion.Content[0].Text.Trim();
//        }
//        else
//        {
//            return "No tourist information found :(";
//        }
//    }
//    catch (Exception ex)
//    {
//        return $"An error occurred: {ex.Message}";
//    }
//}


