namespace BoBit.Fetcher.Models
{
    public class GetBitcoinPriceDto
    {
        public TimeSpan? CacheAge { get; set; }
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string CryptoCurrency { get; set; } = string.Empty;

        public string FiatCurrency { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
