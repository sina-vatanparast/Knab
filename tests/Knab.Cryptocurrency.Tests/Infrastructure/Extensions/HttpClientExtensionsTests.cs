using Newtonsoft.Json;
using Shouldly;
using Knab.Cryptocurrency.Infrastructure.Extensions;
using System.Net;
using System.Text;

namespace Knab.Cryptocurrency.Tests.Infrastructure.Extensions
{
    public class HttpClientExtensionsTests
    {
        [Fact]
        public void ToUrl_WithBaseUrlAndApiUrl_ShouldConstructCorrectUrl()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var apiUrl = "/v1/resources";
            var queryParams = new Dictionary<string, string>
                {
                    { "param1", "value1" },
                    { "param2", "value2" }
                };

            // Act
            var result = queryParams.ToUrl(baseUrl, apiUrl);

            // Assert
            result.ShouldBe("https://api.example.com/v1/resources?param1=value1&param2=value2");
        }

        [Fact]
        public void ToUrl_WithExistingQueryParameters_ShouldAppendNewParameters()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var apiUrl = "/v1/resources?existing=value";
            var queryParams = new Dictionary<string, string>
        {
            { "param1", "value1" }
        };

            // Act
            var result = queryParams.ToUrl(baseUrl, apiUrl);

            // Assert
            result.ShouldBe("https://api.example.com/v1/resources?existing=value&param1=value1");
        }

        [Fact]
        public void ToUrl_WithEmptyQueryParams_ShouldReturnOriginalUrl()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var apiUrl = "/v1/resources";
            var queryParams = new Dictionary<string, string>();

            // Act
            var result = queryParams.ToUrl(baseUrl, apiUrl);

            // Assert
            result.ShouldBe("https://api.example.com/v1/resources");
        }


        [Theory]
        [InlineData("")]
        [InlineData("4343")]
        public void ToUrl_WithInvalidBaseUrl_ShouldThrowArgumentException(string baseUrl)
        {
            // Arrange
            var apiUrl = "/v1/resources";
            var queryParams = new Dictionary<string, string>
        {
            { "param1", "value1" }
        };

            // Act & Assert
            Should.Throw<UriFormatException>(() => queryParams.ToUrl(baseUrl, apiUrl));
        }

        [Fact]
        public async Task ParseJsonResponseAsync_WithValidResponse_ShouldDeserializeCorrectly()
        {
            // Arrange
            var testData = new TestModel { Id = 1, Name = "Test" };
            var content = new StringContent(
                JsonConvert.SerializeObject(testData),
                Encoding.UTF8,
                "application/json"
            );
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            };

            // Act
            var result = await response.ParseJsonResponseAsync<TestModel>(
                "TestEndpoint",
                CancellationToken.None
            );

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.Name.ShouldBe("Test");
        }

        [Fact]
        public async Task ParseJsonResponseAsync_WithNonSuccessStatusCode_ShouldThrowHttpRequestException()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Error message")
            };

            // Act & Assert
            await Should.ThrowAsync<HttpRequestException>(async () =>
                await response.ParseJsonResponseAsync<TestModel>(
                    "TestEndpoint",
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ParseJsonResponseAsync_WithInvalidJson_ShouldThrowJsonException()
        {
            // Arrange
            var content = new StringContent(
                "invalid json",
                Encoding.UTF8,
                "application/json"
            );
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            };

            // Act & Assert
            await Should.ThrowAsync<JsonReaderException>(async () =>
                await response.ParseJsonResponseAsync<TestModel>(
                    "TestEndpoint",
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ParseJsonResponseAsync_WithNullResponse_ShouldThrowException()
        {
            // Arrange
            var content = new StringContent(
                "null",
                Encoding.UTF8,
                "application/json"
            );
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            };

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () =>
                await response.ParseJsonResponseAsync<TestModel>(
                    "TestEndpoint",
                    CancellationToken.None
                )
            );
        }

        private class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
