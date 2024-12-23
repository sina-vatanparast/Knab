using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Knab.Cryptocurrency.Infrastructure.Models
{
    public class CryptoCurrencyQuoteResponse
    {
        [JsonProperty("status")]
        public required StatusInfo Status { get; set; }

        [JsonProperty("data")]
        public required Dictionary<string, CryptoData> Data { get; set; }

        public DateTime GetTimestamp()
        {
            return DateTime.Parse(Status.Timestamp, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }

    }

    public class StatusInfo
    {
        [JsonProperty("timestamp")]
        public required string Timestamp { get; set; }
    }

    public class CryptoData
    {
        [JsonProperty("symbol")]
        public required string BaseCode { get; set; }

        [JsonProperty("name")]
        public required string BaseName { get; set; }

        [JsonProperty("slug")]
        public required string BaseSlug { get; set; }

        [JsonProperty("quote")]
        public required Dictionary<string, QuoteData> Quote { get; set; }
    }

    public class QuoteData
    {
        [JsonExtensionData]
        public required Dictionary<string, JToken> AdditionalData { get; set; }

        public decimal GetPrice()
        {
            return AdditionalData["price"]?.Value<decimal>() ?? 0;
        }
    }
}
