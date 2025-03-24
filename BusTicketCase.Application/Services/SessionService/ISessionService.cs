using BusTicketCase.Application.Services.SessionService.Response;

namespace BusTicketCase.Application.Services.SessionService
{
    public interface ISessionService
    {
        Task<GetSessionResponse> GetSesion();
    }
}
