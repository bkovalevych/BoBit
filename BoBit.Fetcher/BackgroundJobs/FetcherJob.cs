using BoBit.Fetcher.Configs;
using BoBit.Fetcher.Data;
using BoBit.Fetcher.Data.Entities;
using BoBit.Fetcher.Interfaces;
using BoBit.Fetcher.Models;
using Dapper;
using Microsoft.Extensions.Options;

namespace BoBit.Fetcher.BackgroundJobs
{
    public class FetcherJob
        (IBitcoinService bitcoinService,
        IOptions<FetchSettings> optionsDelegate,
        DapperContext ctx,
        ILogger<FetcherJob> logger
        ) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timeDelay = TimeSpan.FromSeconds(optionsDelegate.Value.FetchFrequencyInSeconds);
            logger.LogInformation("Fetcher job started. Frequency: {frequency}", timeDelay);

            while (!stoppingToken.IsCancellationRequested) 
            {
                await Request(timeDelay, stoppingToken);
                await Task.Delay(timeDelay, stoppingToken);
            }
        }

        private async Task Request(TimeSpan timeDelay, CancellationToken ct)
        {
            try
            {
                var resp = await bitcoinService.GetBitcoinPrice(optionsDelegate.Value.Bpi, ct);
                
                if (!resp.IsSuccess) 
                {
                    logger.LogWarning("unsuccesful response");
                    return;
                }

                await SavePrice(resp.Value, ct);

                if (resp.Value.CacheAge != null && resp.Value.CacheAge > timeDelay)
                {
                    logger.LogWarning("Need to wait additional time. It is better to set frequency to {time}s", resp.Value.CacheAge.Value.TotalSeconds);
                    await Task.Delay(resp.Value.CacheAge.Value - timeDelay, ct);
                }

                
            }
            catch (TaskCanceledException)
            {
                logger.LogWarning("Task was cancelled");
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, "exception occured");
            }
            catch (Exception e) 
            {
                logger.LogError(e, "unexpected error");
            }
        }

        private async Task SavePrice(GetBitcoinPriceDto bitcoinPriceDto, CancellationToken ct)
        {
            var sql = @"
              INSERT INTO BitcoinPrices (Created, Timestamp, CryptoCurrency, FiatCurrency, Price)
              VALUES(@Created, @Timestamp, @CryptoCurrency, @FiatCurrency, @Price)";
            
            var entity = new BitcoinPrice()
            {
                Created = bitcoinPriceDto.Created,
                CryptoCurrency = bitcoinPriceDto.CryptoCurrency,
                FiatCurrency = bitcoinPriceDto.FiatCurrency,
                Price = bitcoinPriceDto.Price,
                Timestamp = bitcoinPriceDto.Timestamp,
            };
            
            using var connection = ctx.CreateConnection();
            
            var rows = await connection
                .ExecuteAsync(sql, entity);

            logger.LogInformation("Saved price: {price}", entity.Price);
        }
    }
}
