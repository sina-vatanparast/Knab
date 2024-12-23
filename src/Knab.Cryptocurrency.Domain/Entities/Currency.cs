namespace Knab.Cryptocurrency.Domain.Entities
{
    public class Currency(string code)
    {
        public string Code { get; set; } = code;
    }
}
