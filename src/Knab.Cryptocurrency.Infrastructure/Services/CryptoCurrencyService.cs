using Knab.Cryptocurrency.Domain.Entities;
using Knab.Cryptocurrency.Domain.Interfaces;
using Knab.Cryptocurrency.Domain.ValueObjects;
using Knab.Cryptocurrency.Infrastructure.Clients;
using Knab.Cryptocurrency.Infrastructure.Mappers;

namespace Knab.Cryptocurrency.Infrastructure.Services
{
    public class CryptoCurrencyService(CryptoCurrencyClient cryptoCurrencyClient) : ICryptoCurrencyService
    {
        public async Task<Pair> GetPairAsync(string baseCurrencySlug, string quoteCurrencyCode, CancellationToken cancellationToken)
        {
            var response = await cryptoCurrencyClient.GetLatestQuoteAsync(baseCurrencySlug, quoteCurrencyCode, cancellationToken);

            return response.ToPair();
        }

        public async Task<List<CryptoCurrency>> GetListAsync(int limit, CancellationToken cancellationToken)
        {
            var response = await cryptoCurrencyClient.GetLatestCryptoCurrenciesAsync(limit, cancellationToken);

            return response.ToCryptoCurrency();
        }
    }
}
