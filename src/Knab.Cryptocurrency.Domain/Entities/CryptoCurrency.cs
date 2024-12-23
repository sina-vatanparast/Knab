namespace Knab.Cryptocurrency.Domain.Entities
{
    public class CryptoCurrency(string code, string name, string slug) : Currency(code)
    {
        public string Name { get; set; } = name;
        public string Slug { get; set; } = slug;
    }
}
