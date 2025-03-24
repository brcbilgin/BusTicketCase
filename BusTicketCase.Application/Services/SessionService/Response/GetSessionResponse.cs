using Newtonsoft.Json;
using BusTicketCase.Application.Common;

namespace BusTicketCase.Application.Services.SessionService.Response
{
    public class GetSessionResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public SesionData Data { get; set; }
    }
}
