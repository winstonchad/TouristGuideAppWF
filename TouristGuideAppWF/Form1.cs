using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TouristGuideAppWF.Services;
using System.Diagnostics;

namespace TouristGuideAppWF
{
    public partial class Form1 : Form
    {
        private readonly NominatimService _nominatimService;
        private readonly WeatherService _weatherService;
        private readonly GeminiService _geminiService;
        private readonly HistoryService _historyService;

        public Form1(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();

            HttpClient httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "TouristGuideApp 1.0 (amirkass84@gmail.com) ");

            /// initialize services
            _nominatimService = new NominatimService(httpClient);
            _weatherService = new WeatherService(httpClient, configuration);
            _geminiService = new GeminiService(httpClient, configuration);
            _historyService = new HistoryService();
        }


        //private IConfigurationRoot LoadConfiguration()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        //    return builder.Build();
        //}


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
                var (latitude, longitude) = await _nominatimService.GetCoordinatesAsync(cityName);

                // Вызов методов для получения информации
                string weatherInfo = await _weatherService.GetWeatherAsync(cityName, latitude, longitude);
                string touristInfo = await _geminiService.GetTouristAttractionsAsync(cityName);

                _historyService.AddToHistory(cityName, weatherInfo, touristInfo);

                // Вывод результатов в текстовое поле
                ResultTextBox.Text = $"Weather Info:\n{weatherInfo}\n\nTourist Attractions:\n{touristInfo}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}, {ex.StackTrace}");
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void ResultTextBox_TextChanged(object sender, EventArgs e) { }

        private async void button1_Click(object sender, EventArgs e)
        {
            string cityName = CityNameInput.Text.Trim();

            if (string.IsNullOrEmpty(cityName))
            {
                MessageBox.Show("Please enter a valid city name.");
                return;
            }

            try
            {
                var (latitude, longitude) = await _nominatimService.GetCoordinatesAsync(cityName);

                //string googleMapsUrl = $"https://www.google.com/maps?q={latitude},{longitude}";
                string googleMapsUrl = $"https://www.google.com/maps/place/{cityName}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = googleMapsUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void viewHistoryMenuClick(object sender, EventArgs e)
        {
            HistoryForm historyForm = new HistoryForm(new HistoryService());
            historyForm.Show();
        }
    }
}