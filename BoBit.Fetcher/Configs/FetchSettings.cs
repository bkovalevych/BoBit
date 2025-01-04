namespace BoBit.Fetcher.Configs
{
    public class FetchSettings
    {
        public string Host { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int FetchFrequencyInSeconds { get; set; } = 30;
        
        public string Bpi {  get; set; } = "USD";
    }
}
