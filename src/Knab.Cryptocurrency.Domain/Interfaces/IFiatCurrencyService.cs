using Knab.Cryptocurrency.Domain.ValueObjects;

namespace Knab.Cryptocurrency.Domain.Interfaces
{
    public interface IFiatCurrencyService
    {
        Task<List<Pair>> GetPairsAsync(List<string> quoteCurrencyCodes, CancellationToken cancellationToken);
    }
}
