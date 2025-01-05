using BoBit.Api.Data;
using BoBit.Api.Data.Entities;
using BoBit.Api.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BoBit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitcoinPricesController(
        DapperContext ctx,
        ILogger<BitcoinPricesController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BitcoinPricesResponse>> Get([FromQuery]DateTimeOffset? from, [FromQuery]DateTimeOffset? to)
        {
            if (from == null) 
            {
                from = DateTimeOffset.Now.AddHours(-1);
            }

            if (to == null) 
            {
                to = DateTimeOffset.Now;
            }
            
            IEnumerable<BitcoinPrice>? response = null;
            
            try
            {
                using var connection = ctx.CreateConnection();

                response = await connection.QueryAsync<BitcoinPrice>(@"
            SELECT 
                Timestamp, 
                CryptoCurrency, 
                FiatCurrency, 
                Price 
            FROM BitcoinPrices
            WHERE Timestamp BETWEEN @From AND @To", new { From = from, To = to });
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "db call error");
                return BadRequest("db is not available");
            }

            if (response == null)
            {
                return NotFound();
            }

            var first = response.FirstOrDefault();

            if (first == null) 
            {
                return Ok(new BitcoinPricesResponse(0m, 0m, from.Value, to.Value, string.Empty, string.Empty,
                    []));
            }

            var cryptoCurrency = first.CryptoCurrency?.Trim();
            var fiatCurrency = first.FiatCurrency?.Trim();

            var max = response.Max(x => x.Price);
            var avg = response.Average(x => x.Price);

            var series = response.Select(x => new BitcoinPriceDto(x.Timestamp, x.Price));


            return Ok(new BitcoinPricesResponse(max, avg, from.Value, to.Value, cryptoCurrency, fiatCurrency, series));
        }
    }
}
