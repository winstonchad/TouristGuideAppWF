using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TouristGuideAppWF.Services
{
    /// <summary>
    /// Base service providing common HTTP functionality for all derived services.
    /// </summary>
    public abstract class BaseService
    {
        protected readonly HttpClient _httpClient;
        protected readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes the base service with a shared HttpClient and configuration.
        /// </summary>
        /// <param name="httpClient">The shared HttpClient instance.</param>
        /// <param name="configuration">The configuration for accessing application settings.</param>
        public BaseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // Set a default User-Agent header (can be overridden in derived classes if needed).
            if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "TouristGuideApp/1.0 (contact@example.com)");
            }
        }

        /// <summary>
        /// Sends an HTTP request using the provided HttpRequestMessage and returns the response as a string.
        /// </summary>
        /// <param name="request">The HTTP request to send.</param>
        /// <returns>The response content as a string.</returns>
        protected async Task<string> SendRequestAsync(HttpRequestMessage request)
        {
            try
            {
                // Send the HTTP request
                var response = await _httpClient.SendAsync(request);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Return the response content
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP-related exceptions
                throw new Exception($"HTTP error while making request: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                throw new Exception($"Unexpected error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sends a simple GET request to the specified URL and returns the response as a string.
        /// </summary>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>The response content as a string.</returns>
        protected async Task<string> SendRequestAsync(string url)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("The URL cannot be null or empty.", nameof(url));
            }

            try
            {
                // Send the GET request
                var response = await _httpClient.GetAsync(url);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Return the response content
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP-related exceptions
                throw new Exception($"HTTP error while making request to {url}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                throw new Exception($"Unexpected error while making request to {url}: {ex.Message}", ex);
            }
        }
    }
}
