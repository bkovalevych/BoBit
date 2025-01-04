using BoBit.Fetcher.Configs;
using BoBit.Fetcher.Interfaces;
using BoBit.Fetcher.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace BoBit.Fetcher.IntegrationTests.Services
{
    public class BitcoinServiceTests
    {

        [Fact]
        public async Task GetBitcoinPrice_ValidBpi_ReturnsResponse()
        {
            // Arrange

            var http = new HttpClient() 
            {
                BaseAddress = new Uri("https://api.coindesk.com")
            };

            var options = new Mock<IOptions<FetchSettings>>();

            options.SetupGet(x => x.Value)
                .Returns(new FetchSettings()
                {
                    Path = "/v1/bpi/currentprice.json"
                });

            var dateProvider = new Mock<IDateProvider>();

            dateProvider.SetupGet(x => x.Now)
                .Returns(DateTime.UtcNow);

            var bitcoinService = new BitcoinService(dateProvider.Object, options.Object, http);

            // Act

            var resp = await bitcoinService.GetBitcoinPrice("USD", default);

            // Assert

            Assert.True(resp.IsSuccess);
        }
    }
}
