using BusTicketCase.Application.Services.BusService.Request;
using BusTicketCase.Application.Services.BusService.Response;

namespace BusTicketCase.Application.Services.BusService
{
    public interface IBusService
    {
        Task<GetLocationsResponse> GetBusLocations(GetLocationsRequest request);
        Task<GetJourneysResponse> GetBusJourneys(GetJourneysRequest request);
    }
}
