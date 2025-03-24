using BusTicketCase.MVC.Models;

namespace BusTicketCase.MVC.Business.Interface
{
    public interface IHomeBusiness
    {
        public Task<IndexViewModel> GetBusLocations(bool isReturnPage);
        public Task<JourneyIndexViewModel> GetJourney(IndexViewModel model);
        public Task SetJourneyDetailCache(IndexViewModel model);
        public Task GetBrowserInformation(string userAgent);
    }
}
