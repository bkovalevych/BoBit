namespace BoBit.Api.Models
{
    public record BitcoinPricesResponse(
        decimal MaxPrice, 
        decimal AvgPrice, 
        DateTimeOffset From, 
        DateTimeOffset To,
        string? CryptoCurrency,
        string? FiatCurrency,
        IEnumerable<DateTimeOffset> Labels,
        IEnumerable<decimal> Data);
}
