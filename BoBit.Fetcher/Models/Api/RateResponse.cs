namespace BoBit.Fetcher.Models.Api
{
    public class RateResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Rate { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Rate_float { get; set; }
    }
}
