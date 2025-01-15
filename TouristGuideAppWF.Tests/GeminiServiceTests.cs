using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using TouristGuideAppWF.Services;
using Xunit;

public class GeminiServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly GeminiService _geminiService;

    public GeminiServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://generativelanguage.googleapis.com/")
        };

        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["GoogleAi:ApiKey"]).Returns("fake-api-key");

        _geminiService = new GeminiService(_httpClient, _mockConfiguration.Object);
    }

    [Fact]
    public async Task GetTouristAttractionsAsync_ShouldReturnTouristAttractions_WhenApiReturnsSuccess()
    {
        // Arrange: Mock a successful JSON response from the API
        string fakeApiResponse = @"{
            ""candidates"": [
                {
                    ""content"": {
                        ""parts"": [
                            { ""text"": ""1. Eiffel Tower: Iconic landmark of Paris.\n2. Louvre Museum: Home to the Mona Lisa."" }
                        ]
                    }
                }
            ]
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
                Content = new StringContent(fakeApiResponse, Encoding.UTF8, "application/json")
            });

        // Act
        string result = await _geminiService.GetTouristAttractionsAsync("Paris");

        // Assert
        Assert.Contains("Eiffel Tower", result);
        Assert.Contains("Louvre Museum", result);
    }

    [Fact]
    public async Task GetTouristAttractionsAsync_ShouldReturnErrorMessage_WhenCityNameIsEmpty()
    {
        // Act
        string result = await _geminiService.GetTouristAttractionsAsync("");

        // Assert
        Assert.Equal("City name cannot be empty.", result);
    }

    [Fact]
    public async Task GetTouristAttractionsAsync_ShouldReturnHttpError_WhenApiCallFails()
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
        string result = await _geminiService.GetTouristAttractionsAsync("London");

        // Assert
        Assert.Contains("HTTP error while getting response from Gemini", result);
    }

    [Fact]
    public async Task GetTouristAttractionsAsync_ShouldReturnErrorMessage_WhenApiReturnsEmptyResponse()
    {
        // Arrange: Simulate an empty API response
        string fakeEmptyApiResponse = @"{ ""candidates"": [] }";

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
                Content = new StringContent(fakeEmptyApiResponse, Encoding.UTF8, "application/json")
            });

        // Act
        string result = await _geminiService.GetTouristAttractionsAsync("Tokyo");

        // Assert
        Assert.Equal("No tourist attractions found in Gemini response.", result);
    }
}
