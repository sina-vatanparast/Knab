using Knab.Cryptocurrency.Application.Queries;
using Knab.Cryptocurrency.Domain.Entities;
using Knab.Cryptocurrency.Domain.Interfaces;
using MediatR;

namespace Knab.Cryptocurrency.Application.Handlers
{
    public class GetCryptoCurrenciesQueryHandler(ICryptoCurrencyService cryptoCurrencyService)
        : IRequestHandler<GetCryptoCurrenciesQuery, List<CryptoCurrency>>
    {
        public async Task<List<CryptoCurrency>> Handle(GetCryptoCurrenciesQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await cryptoCurrencyService.GetListAsync(request.Limit, cancellationToken);
        }
    }
}