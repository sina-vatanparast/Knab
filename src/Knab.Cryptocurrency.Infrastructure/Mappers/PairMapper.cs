using Knab.Cryptocurrency.Domain.Entities;
using Knab.Cryptocurrency.Domain.ValueObjects;
using Knab.Cryptocurrency.Infrastructure.Models;

namespace Knab.Cryptocurrency.Infrastructure.Mappers
{
    internal static class PairMapper
    {
        public static Pair ToPair(this CryptoCurrencyQuoteResponse cryptoCurrencyResponse)
        {
            var data = cryptoCurrencyResponse.Data.FirstOrDefault();
            
            var baseCurrency = new Currency(data.Value.BaseCode);

            var quote = data.Value.Quote.FirstOrDefault();

            var quoteCurrency = new Currency(quote.Key);

            return new Pair(baseCurrency, quoteCurrency, quote.Value.GetPrice(), cryptoCurrencyResponse.GetTimestamp());
        }

        public static List<Pair> ToPairs(this FiatExchangeRateListResponse fiatExchangeRateListResponse)
        {
            return fiatExchangeRateListResponse.Rates.Select(p => new Pair(
                new Currency(fiatExchangeRateListResponse.BaseCurrency),
                new Currency(p.Key),
                p.Value,
                DateTimeOffset.FromUnixTimeSeconds(fiatExchangeRateListResponse.Timestamp).UtcDateTime)).ToList();
        }
    }
}
