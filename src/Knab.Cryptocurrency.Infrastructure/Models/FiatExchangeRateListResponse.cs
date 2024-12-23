using Newtonsoft.Json;

namespace Knab.Cryptocurrency.Infrastructure.Models
{
    public class FiatExchangeRateListResponse
    {
        [JsonProperty("base")]
        public required string BaseCurrency { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("rates")]
        public required Dictionary<string, decimal> Rates { get; set; }

    }
}
