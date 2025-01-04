using BoBit.Fetcher.Interfaces;

namespace BoBit.Fetcher.Services
{
    public class DateProvider : IDateProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
