namespace BoBit.Api.Data.Entities
{
    public class BitcoinPrice
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string CryptoCurrency { get; set; } = string.Empty;

        public string FiatCurrency {  get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
