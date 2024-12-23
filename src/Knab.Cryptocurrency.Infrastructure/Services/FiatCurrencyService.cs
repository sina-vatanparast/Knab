using Knab.Cryptocurrency.Domain.Interfaces;
using Knab.Cryptocurrency.Domain.ValueObjects;
using Knab.Cryptocurrency.Infrastructure.Clients;
using Knab.Cryptocurrency.Infrastructure.Mappers;

namespace Knab.Cryptocurrency.Infrastructure.Services
{
    public class FiatCurrencyService(FiatCurrencyClient fiatCurrencyClient) : IFiatCurrencyService
    {
        public async Task<List<Pair>> GetPairsAsync(List<string> quoteCurrencyCodes, CancellationToken cancellationToken)
        {
            var response = await fiatCurrencyClient.GetLatestExchangeRatesAsync(quoteCurrencyCodes, cancellationToken);

            return response.ToPairs();
        }
    }
}
