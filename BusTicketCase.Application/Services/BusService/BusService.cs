using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using BusTicketCase.Application.Common.Cache;
using BusTicketCase.Application.Common.HttpCall;
using BusTicketCase.Application.Configuration;
using BusTicketCase.Application.Services.BusService.Request;
using BusTicketCase.Application.Services.BusService.Response;

namespace BusTicketCase.Application.Services.BusService
{
    public class BusService : IBusService
    {
        private readonly ObiletOptions _obiletOptions;
        private readonly ILogger<BusService> _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IHttpCall _httpCall;

        public BusService(IOptions<ObiletOptions> obiletOptions, ILogger<BusService> logger, ICacheManager cacheManager, IHttpCall httpCall)
        {
            _obiletOptions = obiletOptions.Value;
            _logger = logger;
            _cacheManager = cacheManager;
            _httpCall = httpCall;
        }
        public async Task<GetLocationsResponse> GetBusLocations(GetLocationsRequest request)
        {
            try
            {
                string ApiUrl = $"{_obiletOptions.ApiUri}{_obiletOptions.GetLocations}";
                HttpResponseMessage httpResponse = await _httpCall.PostCall(request, ApiUrl, _obiletOptions.Token);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new GetLocationsResponse() { Status = "Fail" };
                }

                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<GetLocationsResponse>(responseContent);
                if (response != null)
                {
                    response.Data = response.Data.Take(10).ToList();
                } 
                return response ?? new GetLocationsResponse() { Status = "Fail" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBusLocations metodunda hata alındı.");
;                return new GetLocationsResponse() { Status = "Fail" };
            }
        }
        public async Task<GetJourneysResponse> GetBusJourneys(GetJourneysRequest request)
        {
            try
            {
                await _cacheManager.Set("originId", request.Data.OriginId);
                await _cacheManager.Set("destinationId", request.Data.DestinationId);
                await _cacheManager.Set("selectedDate", request.Data.DepartureDate);

                string ApiUrl = $"{_obiletOptions.ApiUri}{_obiletOptions.GetJourneys}";
                HttpResponseMessage httpResponse = await _httpCall.PostCall(request, ApiUrl, _obiletOptions.Token);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new GetJourneysResponse() { Status = "false" };
                }

                string responseContent = await httpResponse.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<GetJourneysResponse>(responseContent);
                if (response != null)
                {
                    response.Data = response.Data.OrderBy(i=> i.Journey.Departure).ToList();
                }
                return response ?? new GetJourneysResponse() { Status = "Fail" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBusJourneys metodunda hata alındı");
                return new GetJourneysResponse() { Status = "Fail" };
            }
        }
    }
}
