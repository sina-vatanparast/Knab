using Shouldly;
using Moq;
using Knab.Cryptocurrency.Application.Handlers;
using Knab.Cryptocurrency.Application.Queries;
using Knab.Cryptocurrency.Domain.Entities;
using Knab.Cryptocurrency.Domain.Interfaces;
using Knab.Cryptocurrency.Domain.ValueObjects;

namespace Knab.Cryptocurrency.Tests.Application.Handlers
{
    public class GetQuotesQueryHandlerTests
    {
        private readonly Mock<ICryptoCurrencyService> _cryptoCurrencyServiceMock;
        private readonly Mock<IFiatCurrencyService> _fiatCurrencyServiceMock;
        private readonly GetQuotesQueryHandler _handler;

        public GetQuotesQueryHandlerTests()
        {
            _cryptoCurrencyServiceMock = new Mock<ICryptoCurrencyService>();
            _fiatCurrencyServiceMock = new Mock<IFiatCurrencyService>();
            _handler = new GetQuotesQueryHandler(_cryptoCurrencyServiceMock.Object, _fiatCurrencyServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnCorrectPairs()
        {
            // Arrange
            var request = new GetQuotesQuery
            {
                BaseCurrencySlug = "bitcoin",
                BaseCurrencyName = "Bitcoin",
                DefaultFiatCurrencyCode = "USD",
                QuoteCurrencyCodes = ["EUR", "GBP"]
            };

            var now = DateTime.Now;

            var defaultPair = new Pair(new Currency("Bitcoin"), new Currency("USD"), 50000m, now);


            var fiatPairs = new List<Pair>
            {
                new(new Currency("USD"), new Currency("EUR"), 0.85m, now),

                new(new Currency("USD"), new Currency("GBP"), 0.73m, now)
            };

            _cryptoCurrencyServiceMock
                .Setup(x => x.GetPairAsync(request.BaseCurrencySlug, request.DefaultFiatCurrencyCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(defaultPair);

            _fiatCurrencyServiceMock
                .Setup(x => x.GetPairsAsync(request.QuoteCurrencyCodes, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fiatPairs);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(3);
            result[0].ShouldBe(defaultPair);

            var eurPair = result[1];
            eurPair.QuoteCurrency.Code.ShouldBe("EUR");
            eurPair.ExchangeRate.ShouldBe(42500m);

            var gbpPair = result[2];
            gbpPair.QuoteCurrency.Code.ShouldBe("GBP");
            gbpPair.ExchangeRate.ShouldBe(36500m);
        }

        [Fact]
        public async Task Handle_EmptyQuotes_ShouldThrowException()
        {
            // Arrange
            var request = new GetQuotesQuery
            {
                BaseCurrencySlug = "bitcoin",
                BaseCurrencyName = "Bitcoin",
                DefaultFiatCurrencyCode = "USD",
                QuoteCurrencyCodes = ["EUR"]
            };

            var defaultPair = new Pair(new Currency("Bitcoin"), new Currency("USD"), 50000m, DateTime.Now);

            _cryptoCurrencyServiceMock
                .Setup(x => x.GetPairAsync(request.BaseCurrencySlug, request.DefaultFiatCurrencyCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(defaultPair);

            _fiatCurrencyServiceMock
                .Setup(x => x.GetPairsAsync(request.QuoteCurrencyCodes, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Pair>());

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () =>
                await _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_MismatchedBaseCurrency_ShouldThrowException()
        {
            // Arrange
            var request = new GetQuotesQuery
            {
                BaseCurrencySlug = "bitcoin",
                BaseCurrencyName = "Bitcoin",
                DefaultFiatCurrencyCode = "USD",
                QuoteCurrencyCodes = ["EUR"]
            };
            var now = DateTime.Now;

            var defaultPair = new Pair(new Currency("Bitcoin"), new Currency("USD"), 50000m, now);

            var fiatPairs = new List<Pair>
            {
                 new(new Currency("EUR"), new Currency("GBP"), 0.85m, now)
            };

            _cryptoCurrencyServiceMock
                .Setup(x => x.GetPairAsync(request.BaseCurrencySlug, request.DefaultFiatCurrencyCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(defaultPair);

            _fiatCurrencyServiceMock
                .Setup(x => x.GetPairsAsync(request.QuoteCurrencyCodes, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fiatPairs);

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () =>
                await _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_CancellationRequested_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var request = new GetQuotesQuery
            {
                BaseCurrencySlug = "bitcoin",
                BaseCurrencyName = "Bitcoin",
                DefaultFiatCurrencyCode = "USD",
                QuoteCurrencyCodes = ["EUR"]
            };

            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Should.ThrowAsync<OperationCanceledException>(async () =>
                await _handler.Handle(request, cts.Token));
        }
    }
}
