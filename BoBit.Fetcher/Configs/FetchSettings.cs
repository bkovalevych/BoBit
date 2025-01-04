namespace BoBit.Fetcher.Configs
{
    public class FetchSettings
    {
        public string Host { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int FetchFrequencyInSeconds { get; set; } = 30;
        public int RetryDelayInSeconds { get; set; } = 5;
        public int RetryCount { get; set; } = 3;
    }
}
