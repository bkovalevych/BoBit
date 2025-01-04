using Ardalis.Result;
using BoBit.Fetcher.Configs;
using BoBit.Fetcher.Interfaces;
using BoBit.Fetcher.Models;
using BoBit.Fetcher.Models.Api;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace BoBit.Fetcher.Services
{
    public class BitcoinService(
        IDateProvider dateProvider,
        IOptions<FetchSettings> fetchSettingsDelegate,
        HttpClient http) : IBitcoinService
    {
        public const string InvalidBpiMessage = "Bpi '{0}' is invalid. Valid values are: USD, GBP, EUR";
        private readonly Regex ValidBpiRegex = new Regex("^USD|GBP|EUR$");


        public async Task<Result<GetBitcoinPriceDto>> GetBitcoinPrice(string bpi, CancellationToken ct)
        {
            var validation = ValidateBpi(bpi);

            if (!validation.Succesful)
            {
                return Result<GetBitcoinPriceDto>
                    .Invalid(new ValidationError("bpi", validation.Error));
            }

            var path = fetchSettingsDelegate.Value.Path;
            var response = await http.GetAsync(path, ct);

            response.EnsureSuccessStatusCode();

            var bitcoinPriceResponse = await response.Content
                .ReadFromJsonAsync<BitcoinPriceResponse>(ct);

            if (bitcoinPriceResponse == null)
            {
                return Result<GetBitcoinPriceDto>.Error("Parsing error");
            }

            if (bitcoinPriceResponse.Bpi.TryGetValue(bpi, out var rateResponse))
            {
                return Result.Success(new GetBitcoinPriceDto()
                {
                    CacheAge = response.Headers.Age,
                    Created = dateProvider.Now,
                    CryptoCurrency = bitcoinPriceResponse.ChartName,
                    FiatCurrency = bpi,
                    Price = (decimal)rateResponse.Rate_float,
                    Timestamp = bitcoinPriceResponse.Time.UpdatedISO
                });
            }

            return Result<GetBitcoinPriceDto>.NotFound(); ;
        }

        private (bool Succesful, string Error) ValidateBpi(string bpi)
        {
            if (!ValidBpiRegex.IsMatch(bpi))
            {
                return (false, string.Format(InvalidBpiMessage, bpi));
            }

            return (true, string.Empty);
        }
    }
}
