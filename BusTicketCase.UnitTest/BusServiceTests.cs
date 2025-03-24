using FizzWare.NBuilder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using BusTicketCase.Application.Common.Cache;
using BusTicketCase.Application.Common.HttpCall;
using BusTicketCase.Application.Configuration;
using BusTicketCase.Application.Services.BusService;
using BusTicketCase.Application.Services.BusService.Request;
using BusTicketCase.Application.Services.BusService.Response;
using System.Net;
using System.Text;

namespace BusTicketCase.UnitTest
{
    [TestFixture]
    public class BusServiceTests
    {
        private Mock<IOptions<ObiletOptions>> _optionsMock;
        private Mock<ILogger<BusService>> _loggerMock;
        private Mock<ICacheManager> _cacheManagerMock;
        private Mock<IHttpCall> _httpCallMock;
        private BusService _busService;

        [SetUp]
        public void SetUp()
        {
            _optionsMock = new Mock<IOptions<ObiletOptions>>();
            _loggerMock = new Mock<ILogger<BusService>>();
            _cacheManagerMock = new Mock<ICacheManager>();
            _httpCallMock = new Mock<IHttpCall>();

            _busService = new BusService(_optionsMock.Object, _loggerMock.Object, _cacheManagerMock.Object, _httpCallMock.Object);
        }

        [Test]
        public async Task GetBusLocations_WhenHttpResponseIsSuccessful_ShouldReturnValidResponse()
        {
            // Arrange
            var request = Builder<GetLocationsRequest>.CreateNew().Build();
            var response = new GetLocationsResponse
            {
                Status = "Success",
                Data = new List<LocationData>()
            };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(response));

            _httpCallMock.Setup(x => x.PostCall(It.IsAny<GetLocationsRequest>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _busService.GetBusLocations(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Success", result.Status);
            Assert.IsEmpty(result.Data);
        }

        [Test]
        public async Task GetBusLocations_WhenHttpResponseIsNotSuccessful_ShouldReturnFailResponse()
        {
            var request = Builder<GetLocationsRequest>.CreateNew().Build();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            _httpCallMock.Setup(x => x.PostCall(It.IsAny<GetLocationsRequest>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(httpResponseMessage);

            var result = await _busService.GetBusLocations(request);

            Assert.IsNotNull(result);
            Assert.AreEqual("Fail", result.Status);
        }
    }
}