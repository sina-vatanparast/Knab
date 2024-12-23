using Knab.Cryptocurrency.Domain.ValueObjects;
using MediatR;

namespace Knab.Cryptocurrency.Application.Queries
{
    public class GetQuotesQuery : IRequest<List<Pair>>
    {
        public required string BaseCurrencyName { get; set; }
        public required string BaseCurrencySlug { get; set; }
        public required string DefaultFiatCurrencyCode { get; set; }
        public required List<string> QuoteCurrencyCodes { get; set; }
    }
}
