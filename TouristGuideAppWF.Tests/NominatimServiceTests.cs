using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TouristGuideAppWF.Services;
using Xunit;
using Moq;
using Moq.Protected;

public class NominatimServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly NominatimService _nominatimService;

    public NominatimServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://nominatim.openstreetmap.org/")
        };

        _nominatimService = new NominatimService(_httpClient);
    }

    [Fact]
    public async Task GetCoordinatesAsync_ShouldReturnCoordinates_WhenCityExists()
    {
        // Arrange
        string cityName = "London";
        string jsonResponse = "[{\"lat\": \"51.5074\", \"lon\": \"-0.1278\"}]";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var (latitude, longitude) = await _nominatimService.GetCoordinatesAsync(cityName);

        // Assert
        Assert.Equal(51.5074, latitude);
        Assert.Equal(-0.1278, longitude);
    }

    [Fact]
    public async Task GetCoordinatesAsync_ShouldThrowException_WhenCityNotFound()
    {
        // Arrange
        string cityName = "UnknownCity";
        string jsonResponse = "[]"; // API возвращает пустой массив, если город не найден

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _nominatimService.GetCoordinatesAsync(cityName));
        Assert.Contains("City 'UnknownCity' not found", exception.Message);
    }

    [Fact]
    public async Task GetCoordinatesAsync_ShouldThrowHttpException_WhenApiFails()
    {
        // Arrange
        string cityName = "Paris";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _nominatimService.GetCoordinatesAsync(cityName));
        Assert.Contains("HTTP error while getting coordinates", exception.Message);
    }
}
