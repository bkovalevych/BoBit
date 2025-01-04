namespace BoBit.Fetcher.Models.Api
{
    public class BitcoinPriceResponse
    {
        public string ChartName { get; set; } = string.Empty; // Bitcoin

        public Dictionary<string, RateResponse> Bpi { get; set; } = [];

        public TimeResponse Time { get; set; } = new TimeResponse();
    }
}
