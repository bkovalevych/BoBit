using Ardalis.Result;
using BoBit.Fetcher.Models;

namespace BoBit.Fetcher.Interfaces
{
    public interface IBitcoinService
    {
        /// <summary>
        /// Get data produced from the CoinDesk Bitcoin Price Index
        /// </summary>
        /// <param name="bpi">Valid values: USD, GBP, EUR</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Crypto currency rate according to parameter of bpi</returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        Task<Result<GetBitcoinPriceDto>> GetBitcoinPrice(string bpi, CancellationToken ct);
    }
}
