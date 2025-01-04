using BoBit.Fetcher.Configs;
using BoBit.Fetcher.Interfaces;
using BoBit.Fetcher.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net.Http.Json;
using System.Net;

namespace BoBit.Fetcher.UnitTests.Services
{
    public class BitcoinServiceTests
    {
        
        [Fact]
        public async Task GetBitcoinPrice_InvalidBpi_ReturnsInvalidResultWithMessage()
        {
            // Arrange

            var fakeSettings = new Mock<IOptions<FetchSettings>>();
            var fakeDateProvider = new Mock<IDateProvider>();
            var fakeHttpClient = new Mock<HttpClient>();

            var bitcoinService = new BitcoinService(
                fakeDateProvider.Object,
                fakeSettings.Object,
                fakeHttpClient.Object);
            
            var invalidBpi = "invalid";

            // Act

            var result = await bitcoinService.GetBitcoinPrice(invalidBpi, default);

            // Assert

            Assert.Contains(result.ValidationErrors, x => x.ErrorMessage == string.Format(BitcoinService.InvalidBpiMessage, invalidBpi));
        }

        [Fact]
        public async Task GetBitcoinPrice_ValidBpi_ReturnsResultWithBadStatusCode()
        {
            // Arrange

            var fakeSettings = new Mock<IOptions<FetchSettings>>();

            fakeSettings.SetupGet(x => x.Value)
                .Returns(new FetchSettings()
                {
                    Path = "/path"
                });

            var fakeDateProvider = new Mock<IDateProvider>();
            var fakeHttpHandler = new Mock<HttpMessageHandler>();

            fakeHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Content = new StringContent("{}")
                });
            
            var fakeHttpClient = new HttpClient(fakeHttpHandler.Object)
            {
                BaseAddress = new Uri("https://api")
            };

            var bitcoinService = new BitcoinService(
                fakeDateProvider.Object,
                fakeSettings.Object,
                fakeHttpClient);

            var bpi = "USD";

            // Act
            Func<Task> func = () => bitcoinService.GetBitcoinPrice(bpi, default);

            // Assert

            await Assert.ThrowsAsync<HttpRequestException>(func);
            
            
        }
    }
}
