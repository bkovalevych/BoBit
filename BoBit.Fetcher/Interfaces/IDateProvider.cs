namespace BoBit.Fetcher.Interfaces
{
    public interface IDateProvider
    {
        public DateTimeOffset Now { get; }
    }
}
