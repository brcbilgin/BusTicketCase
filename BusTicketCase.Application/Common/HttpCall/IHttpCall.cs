using Newtonsoft.Json;
using BusTicketCase.Application.Services.SessionService.Request;

namespace BusTicketCase.Application.Common.HttpCall
{
    public interface IHttpCall
    {
        Task<HttpResponseMessage> PostCall<T>(T request, string ApiUrl, string token) where T : class;
    }
}
