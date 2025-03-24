using Newtonsoft.Json;
using BusTicketCase.Application.Services.BusService.Common;

namespace BusTicketCase.Application.Services.BusService.Request
{
    public class GetLocationsRequest : BaseRequest
    {
        [JsonProperty("data")]
        public string? Data { get; set; }
    }
}
