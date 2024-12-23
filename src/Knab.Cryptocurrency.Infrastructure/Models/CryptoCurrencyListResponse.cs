using Newtonsoft.Json;

namespace Knab.Cryptocurrency.Infrastructure.Models
{
    public class CryptoCurrencyListResponse
    {
        [JsonProperty("data")]
        public required List<CryptoCurrencyResponse> Data { get; set; }

    }

    public class CryptoCurrencyResponse
    {
        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("symbol")]
        public required string Symbol { get; set; }

        [JsonProperty("slug")]
        public required string Slug { get; set; }

    }
}
