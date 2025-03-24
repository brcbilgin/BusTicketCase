using Newtonsoft.Json;

namespace BusTicketCase.Application.Common
{
    public class SesionData
    {
        [JsonProperty("session-id")]
        public string SessionId { get; set; }

        [JsonProperty("device-id")]
        public string DeviceId { get; set; }
    }
}
