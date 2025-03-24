using Newtonsoft.Json;

namespace BusTicketCase.Application.Services.BusService.Response
{
    public class GetLocationsResponse
    {
        public GetLocationsResponse()
        {
            Data = new List<LocationData>();
        }

        [JsonProperty("status")]
        public required string Status { get; set; }
        [JsonProperty("data")]
        public List<LocationData> Data { get; set; }
    }

    public class LocationData
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public required string Name { get; set; }
    }
}
