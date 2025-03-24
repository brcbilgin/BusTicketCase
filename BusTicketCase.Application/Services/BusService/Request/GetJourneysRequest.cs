using Newtonsoft.Json;
using BusTicketCase.Application.Services.BusService.Common;

namespace BusTicketCase.Application.Services.BusService.Request
{
    public class GetJourneysRequest : BaseRequest
    {
        [JsonProperty("data")]
        public required Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("origin-id")]
        public int OriginId { get; set; }

        [JsonProperty("destination-id")]
        public int DestinationId { get; set; }

        [JsonProperty("departure-date")]
        public DateTime DepartureDate { get; set; }
    }
}
