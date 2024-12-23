using Knab.Cryptocurrency.Domain.Entities;
using Knab.Cryptocurrency.Domain.ValueObjects;

namespace Knab.Cryptocurrency.Domain.Interfaces
{
    public interface ICryptoCurrencyService
    {
        Task<Pair> GetPairAsync(string baseCurrencySlug, string quoteCurrencyCode, CancellationToken cancellationToken);

        Task<List<CryptoCurrency>> GetListAsync(int limit, CancellationToken cancellationToken);
    }
}
