using Knab.Cryptocurrency.Application.Queries;
using Knab.Cryptocurrency.Domain.Entities;
using Knab.Cryptocurrency.Domain.Interfaces;
using Knab.Cryptocurrency.Domain.ValueObjects;
using MediatR;

namespace Knab.Cryptocurrency.Application.Handlers
{
    public class GetQuotesQueryHandler(ICryptoCurrencyService cryptoCurrencyService, IFiatCurrencyService fiatCurrencyService)
        : IRequestHandler<GetQuotesQuery, List<Pair>>
    {
        public async Task<List<Pair>> Handle(GetQuotesQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var defaultPair = await cryptoCurrencyService.GetPairAsync(request.BaseCurrencySlug, request.DefaultFiatCurrencyCode, cancellationToken);

            var quotes = await fiatCurrencyService.GetPairsAsync(request.QuoteCurrencyCodes, cancellationToken);

            if(quotes?.Any() != true)
            {
                throw new Exception("No quotes are available for this currency");
            }

            if(request.DefaultFiatCurrencyCode != quotes.First().BaseCurrency.Code)
            {
                throw new Exception("The default fiat value in setting is not supported");
            }

            var defaultExchangeRate = defaultPair.ExchangeRate;

            foreach ( var pair in quotes)
            {
                pair.BaseCurrency = new Currency(request.BaseCurrencyName);
                pair.ExchangeRate *= defaultExchangeRate;
            }

            return quotes;
        }
    }
}