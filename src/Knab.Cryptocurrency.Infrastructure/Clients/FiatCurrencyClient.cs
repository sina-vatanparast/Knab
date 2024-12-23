using Knab.Cryptocurrency.Domain.Settings;
using Knab.Cryptocurrency.Infrastructure.Extensions;
using Knab.Cryptocurrency.Infrastructure.Models;

namespace Knab.Cryptocurrency.Infrastructure.Clients
{
    public class FiatCurrencyClient(HttpClient httpClient, ApiSettings apiSettings)
    {
        public async Task<FiatExchangeRateListResponse> GetLatestExchangeRatesAsync(List<string> fiatCurrencyCodes,CancellationToken cancellationToken)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "access_key", apiSettings.ApiKey },
                { "symbols", string.Join(",", fiatCurrencyCodes)},
            };

            var url = queryParams.ToUrl(apiSettings.BaseUrl, "v1/latest");

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (response is null)
            {
                throw new Exception("Failed to fetch the latest quotes from fiat currency server.");
            }

            return await response.ParseJsonResponseAsync<FiatExchangeRateListResponse>("latest exchange rates", cancellationToken);
        }
    }
}
