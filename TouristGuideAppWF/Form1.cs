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
        private readonly NominatimService _nominatimService; // Service for retrieving coordinates
        private readonly WeatherService _weatherService; // Service for retrieving weather data
        private readonly GeminiService _geminiService; // Service for retrieving tourist attractions
        private readonly HistoryService _historyService; // Service for managing search history

        public Form1(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();

            // Create an HTTP client and set a user-agent header
            HttpClient httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "TouristGuideApp 1.0 (amirkass84@gmail.com)");

            // Initialize the services
            _nominatimService = new NominatimService(httpClient, configuration);
            _weatherService = new WeatherService(httpClient, configuration);
            _geminiService = new GeminiService(httpClient, configuration);
            _historyService = new HistoryService();
        }

        /// <summary>
        /// Handles the search button click event. Fetches coordinates, weather, and tourist info for the entered city.
        /// </summary>
        private async void SearchBtn_Click(object sender, EventArgs e)
        {
            string cityName = CityNameInput.Text.Trim(); // Get the city name from the input

            if (string.IsNullOrEmpty(cityName))
            {
                MessageBox.Show("Please enter a valid city name.");
                return;
            }

            try
            {
                // Retrieve coordinates for the city
                var (latitude, longitude) = await _nominatimService.GetCoordinatesAsync(cityName);

                // Fetch weather and tourist information
                string weatherInfo = await _weatherService.GetWeatherAsync(cityName, latitude, longitude);
                string touristInfo = await _geminiService.GetTouristAttractionsAsync(cityName);

                // Add the result to the search history
                _historyService.AddToHistory(cityName, weatherInfo, touristInfo);

                // Display the results in the text box
                ResultTextBox.Text = $"Weather Info:\n{weatherInfo}\n\nTourist Attractions:\n{touristInfo}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}, {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Opens the city location in Google Maps when the button is clicked.
        /// </summary>
        private async void button1_Click(object sender, EventArgs e)
        {
            string cityName = CityNameInput.Text.Trim(); // Get the city name from the input

            if (string.IsNullOrEmpty(cityName))
            {
                MessageBox.Show("Please enter a valid city name.");
                return;
            }

            try
            {
                // Retrieve coordinates (even though they're not directly used here)
                var (latitude, longitude) = await _nominatimService.GetCoordinatesAsync(cityName);

                // Construct the Google Maps URL
                string googleMapsUrl = $"https://www.google.com/maps/place/{cityName}";

                // Open the URL in the default browser
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

        /// <summary>
        /// Opens the search history window when the "View History" menu item is clicked.
        /// </summary>
        private void viewHistoryMenuClick(object sender, EventArgs e)
        {
            HistoryForm historyForm = new HistoryForm(new HistoryService()); // Open the history form
            historyForm.Show();
        }

        // Unused event handlers removed as dead code
    }
}
