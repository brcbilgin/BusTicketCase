using Newtonsoft.Json;
using BusTicketCase.Application.Services.BusService.Response;

namespace BusTicketCase.MVC.Models
{
    public class JourneyIndexViewModel
    {
        public JourneyIndexViewModel()
        {
            Journeys = new List<JourneyModel>();
        }
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string SelectedDate { get; set; }
        public List<JourneyModel> Journeys { get; set; }
    }
    public class JourneyModel
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Currency { get; set; }
        public string Price { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
    }
}
