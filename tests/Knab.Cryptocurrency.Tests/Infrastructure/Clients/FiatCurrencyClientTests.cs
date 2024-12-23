using Knab.Cryptocurrency.Domain.Settings;
using Knab.Cryptocurrency.Infrastructure.Clients;
using Moq.Protected;
using Moq;
using Shouldly;
using System.Net;

namespace Knab.Cryptocurrency.Tests.Infrastructure.Clients
{
    public class FiatCurrencyClientTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public FiatCurrencyClientTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _apiSettings = new ApiSettings
            {
                ApiKey = "test-api-key",
                BaseUrl = "https://your-api.com/"
            };
        }

        [Fact]
        public async Task GetLatestExchangeRatesAsync_WithAcceptableResponse_ShouldReturnExchangeRatesResponse()
        {
            // Arrange
            var fiatCurrencyCodes = new List<string> { "USD", "EUR", "GBP" };
            var mockResponseContent = @"
        {
            ""success"": true,
            ""rates"": {
                ""USD"": 1.0,
                ""EUR"": 0.85,
                ""GBP"": 0.75
            }
        }";

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(mockResponseContent)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            var client = new FiatCurrencyClient(_httpClient, _apiSettings);

            // Act
            var result = await client.GetLatestExchangeRatesAsync(fiatCurrencyCodes, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Rates.ShouldNotBeNull();
            result.Rates["USD"].ShouldBe(1.0m);
            result.Rates["EUR"].ShouldBe(0.85m);
            result.Rates["GBP"].ShouldBe(0.75m);
        }

        [Fact]
        public async Task GetLatestExchangeRatesAsync_WithNullResponse_ShouldThrowException()
        {
            // Arrange
            var fiatCurrencyCodes = new List<string> { "USD", "EUR", "GBP" };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws(new Exception("Simulated failure")); // Simulate failure

            var client = new FiatCurrencyClient(_httpClient, _apiSettings);

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () =>
                await client.GetLatestExchangeRatesAsync(fiatCurrencyCodes, CancellationToken.None));
        }

        [Fact]
        public async Task GetLatestExchangeRatesAsync_OnInternalServerError_ShouldThrowException()
        {
            // Arrange
            var fiatCurrencyCodes = new List<string> { "USD", "EUR", "GBP" };

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("{ \"error\": \"Internal Server Error\" }")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            var client = new FiatCurrencyClient(_httpClient, _apiSettings);

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () =>
                await client.GetLatestExchangeRatesAsync(fiatCurrencyCodes, CancellationToken.None));
        }

    }

}
