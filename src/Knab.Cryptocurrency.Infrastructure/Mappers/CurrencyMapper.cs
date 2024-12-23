using Knab.Cryptocurrency.Domain.Entities;
using Knab.Cryptocurrency.Infrastructure.Models;

namespace Knab.Cryptocurrency.Infrastructure.Mappers
{
    internal static class CurrencyMapper
    {
        public static List<CryptoCurrency> ToCryptoCurrency(this CryptoCurrencyListResponse CryptoCurrencyListResponse) =>
            CryptoCurrencyListResponse.Data.Select(p => new CryptoCurrency(p.Symbol, p.Name, p.Slug)).ToList();
    }
}
