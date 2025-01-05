namespace BoBit.Api.Models
{
    public record BitcoinPricesResponse(
        decimal MaxPrice, 
        decimal AvgPrice, 
        DateTimeOffset From, 
        DateTimeOffset To,
        string? CryptoCurrency,
        string? FiatCurrency,
        IEnumerable<BitcoinPriceDto> Series);

    public record BitcoinPriceDto(DateTimeOffset Name, decimal Value);
}
