using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using TouristGuideAppWF.Services;
using Xunit;

public class WeatherServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly WeatherService _weatherService;

    public WeatherServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.openweathermap.org/")
        };

        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["WeatherApi:ApiKey"]).Returns("fake-api-key");

        _weatherService = new WeatherService(_httpClient, _mockConfiguration.Object);
    }

    [Fact]
    public async Task GetWeatherAsync_ShouldReturnWeatherInfo_WhenApiReturnsSuccess()
    {
        // Arrange: Mock the API response
        string fakeApiResponse = @"{
            ""weather"": [{""description"": ""clear sky""}],
            ""main"": {""temp"": 25.5}
        }";

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeApiResponse)
            });

        // Act: Call the method
        string result = await _weatherService.GetWeatherAsync("New York", 40.7128, -74.0060);

        // Assert: Verify the result
        Assert.Contains("The weather in New York is clear sky", result);
        Assert.Contains("temperature is 25.5°C", result);
    }

    [Fact]
    public async Task GetWeatherAsync_ShouldReturnError_WhenCityNameIsEmpty()
    {
        // Act
        string result = await _weatherService.GetWeatherAsync("", 40.7128, -74.0060);

        // Assert
        Assert.Equal("City name is missing.", result);
    }

    [Fact]
    public async Task GetWeatherAsync_ShouldReturnHttpError_WhenApiCallFails()
    {
        // Arrange: Simulate a network failure
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network failure"));

        // Act
        string result = await _weatherService.GetWeatherAsync("Los Angeles", 34.0522, -118.2437);

        // Assert
        Assert.Contains("HTTP error while fetching weather", result);
    }
}
