using Knab.Cryptocurrency.Domain.Entities;
using MediatR;

namespace Knab.Cryptocurrency.Application.Queries
{
    public class GetCryptoCurrenciesQuery : IRequest<List<CryptoCurrency>>
    {
        public int Limit { get; set; }
    }
}
