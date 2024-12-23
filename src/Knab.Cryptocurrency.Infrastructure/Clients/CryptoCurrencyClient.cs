using Knab.Cryptocurrency.Domain.Settings;
using Knab.Cryptocurrency.Infrastructure.Extensions;
using Knab.Cryptocurrency.Infrastructure.Models;

namespace Knab.Cryptocurrency.Infrastructure.Clients
{
    public class CryptoCurrencyClient
    {
        private readonly HttpClient httpClient;
        private readonly ApiSettings apiSettings;

        public CryptoCurrencyClient(HttpClient httpClient, ApiSettings apiSettings)
        {
            this.httpClient = httpClient;
            this.apiSettings = apiSettings;
            
            this.httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", this.apiSettings.ApiKey);
        }

        public async Task<CryptoCurrencyQuoteResponse> GetLatestQuoteAsync(string baseCurrencySlug, string quoteCurrencyCode, CancellationToken cancellationToken)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "convert", quoteCurrencyCode },
                { "slug", baseCurrencySlug },
                { "aux", "is_active" },
            };

            var url = queryParams.ToUrl(apiSettings.BaseUrl, "v2/cryptocurrency/quotes/latest");

            var response = await httpClient.GetAsync(url, cancellationToken);
            
            if (response is null)
            {
                throw new Exception("Failed to fetch the latest quote from crypto currency server.");
            }

            return await response.ParseJsonResponseAsync<CryptoCurrencyQuoteResponse>("latest quote", cancellationToken);
        }

        public async Task<CryptoCurrencyListResponse> GetLatestCryptoCurrenciesAsync(int limit, CancellationToken cancellationToken)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "start", "1" },
                { "limit", limit.ToString() },
                { "convert", "EUR" },
                { "aux", "is_market_cap_included_in_calc" },
            };

            var url = queryParams.ToUrl(apiSettings.BaseUrl, "v1/cryptocurrency/listings/latest");

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (response is null)
            {
                throw new Exception("Failed to fetch the latest cryptocurrencies from crypto currency server.");
            }

            return await response.ParseJsonResponseAsync<CryptoCurrencyListResponse>("currency list", cancellationToken);
        }
    }
}
