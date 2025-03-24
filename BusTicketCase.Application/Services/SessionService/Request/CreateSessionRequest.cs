using Newtonsoft.Json;
using BusTicketCase.Application.Common;

namespace BusTicketCase.Application.Services.SessionService.Request
{
    public class GetSessionRequest
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("connection")]
        public Connection ConnectionInfo { get; set; }
        [JsonProperty("browser")]
        public Browser BrowserInfo { get; set; }
        public class Connection
        {
            [JsonProperty("ip-address")]
            public string IpAddress { get; set; }
            [JsonProperty("port")]
            public string Port { get; set; }
        }
        public class Browser
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }
        }
    }
}
