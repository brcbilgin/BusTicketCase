using Newtonsoft.Json;
using BusTicketCase.Application.Common.Convertor;

namespace BusTicketCase.Application.Services.BusService.Response
{
    public class GetJourneysResponse
    {
        public GetJourneysResponse()
        {
            Data = new List<JourneyData>();
        }
        [JsonProperty("status")]
        public required string Status { get; set; }
        [JsonProperty("data")]
        public List<JourneyData> Data { get; set; }
    }
    public class JourneyData
    {
        [JsonProperty("journey")]
        public Journey Journey { get; set; }
    }
    public class Journey
    {
        [JsonProperty("origin")]
        public string Origin { get; set; }
        [JsonProperty("destination")]
        public string Destination { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("internet-price")]
        public string Price { get; set; }
        [JsonProperty("arrival")]
        [JsonConverter(typeof(DateTimeConvertor))]
        public DateTime Arrival { get; set; }
        [JsonProperty("departure")]
        [JsonConverter(typeof(DateTimeConvertor))]
        public DateTime Departure { get; set; }
    }
}
