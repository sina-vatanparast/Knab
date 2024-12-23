using Knab.Cryptocurrency.Domain.Entities;

namespace Knab.Cryptocurrency.Domain.ValueObjects
{
    public class Pair(Currency baseCurrency, Currency quoteCurrency, decimal exchangeRate, DateTime timestamp)
    {
        public Currency BaseCurrency { set; get; } = baseCurrency;
        public Currency QuoteCurrency { set; get; } = quoteCurrency;
        public decimal ExchangeRate
        {
            get => Math.Round(exchangeRate, 2);
            set => exchangeRate = value;
        }
        public DateTime Timestamp { get; set; } = timestamp;
    }
}
