namespace Knab.Cryptocurrency.Domain.Settings
{
    public class AppSettings
    {
        public required ApiSettings CryptoCurrencyApi { get; set; }
        public required ApiSettings FiatCurrencyApi { get; set; }
        public required List<string> FiatCurrencyCodes { get; set; }
        public required string DefaultFiatCurrencyCode { get; set; }
        public int CryptoCurrenciesLimit { get; set; }

    }

    public class ApiSettings
    {
        public required string BaseUrl {  get; set; }
        public required string ApiKey {  get; set; }
    }
}
