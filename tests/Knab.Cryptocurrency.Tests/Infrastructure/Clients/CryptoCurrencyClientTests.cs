using System.Net;
using Knab.Cryptocurrency.Domain.Settings;
using Knab.Cryptocurrency.Infrastructure.Clients;
using Moq;
using Moq.Protected;
using Shouldly;

namespace Knab.Cryptocurrency.Tests.Infrastructure.Clients
{
    public class CryptoCurrencyClientTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public CryptoCurrencyClientTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _apiSettings = new ApiSettings
            {
                ApiKey = "test-api-key",
                BaseUrl = "https://my-api.com/"
            };
        }

        [Fact]
        public async Task GetLatestQuoteAsync_WithAcceptableResponse_ShouldReturnQuoteResponse()
        {
            // Arrange
            var baseCurrencySlug = "bitcoin";
            var quoteCurrencyCode = "USD";
            var mockResponseContent = @"
    {
        ""data"": {
            ""bitcoin"": {
                ""id"": 1,
                ""name"": ""Bitcoin"",
                ""symbol"": ""BTC"",
                ""quote"": {
                    ""USD"": {
                        ""price"": 50000.0,
                        ""volume_24h"": 1000000000.0,
                        ""percent_change_1h"": 0.1
                    }
                }
            }
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

            var client = new CryptoCurrencyClient(_httpClient, _apiSettings);

            // Act
            var result = await client.GetLatestQuoteAsync(baseCurrencySlug, quoteCurrencyCode, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            result.Data["bitcoin"].Quote["USD"].GetPrice().ShouldBe(50000.0m);
        }

        [Fact]
        public async Task GetLatestQuoteAsync_WithNullResponse_ShouldThrowException()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => null!); // Explicitly specify null with null-forgiving operator

            var client = new CryptoCurrencyClient(_httpClient, _apiSettings);

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () =>
                await client.GetLatestQuoteAsync("bitcoin", "USD", CancellationToken.None));
        }

        [Fact]
        public async Task GetLatestCryptoCurrenciesAsync_WithAcceptableResponse_ShouldReturnCryptoCurrencyListResponse()
        {
            // Arrange
            var limit = 10;
            var mockResponseContent = @"
    {
        ""data"": [
            {
                ""id"": 1,
                ""name"": ""Bitcoin"",
                ""symbol"": ""BTC"",
                ""is_active"": true
            },
            {
                ""id"": 2,
                ""name"": ""Ethereum"",
                ""symbol"": ""ETH"",
                ""is_active"": true
            }
        ]
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

            var client = new CryptoCurrencyClient(_httpClient, _apiSettings);

            // Act
            var result = await client.GetLatestCryptoCurrenciesAsync(limit, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(2); // Ensure we have 2 items in the mocked response
            result.Data[0].Name.ShouldBe("Bitcoin");
        }


        [Fact]
        public async Task GetLatestCryptoCurrenciesAsync_WithNullResponse_ShouldThrowException()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => null!); // Explicitly specify null with null-forgiving operator

            var client = new CryptoCurrencyClient(_httpClient, _apiSettings);

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () =>
                await client.GetLatestCryptoCurrenciesAsync(10, CancellationToken.None));
        }

    }

}
