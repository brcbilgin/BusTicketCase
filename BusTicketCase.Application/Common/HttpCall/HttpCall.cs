using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using BusTicketCase.Application.Configuration;
using BusTicketCase.Application.Services.SessionService.Request;

namespace BusTicketCase.Application.Common.HttpCall
{
    public class HttpCall : IHttpCall
    {
        private readonly ILogger<HttpCall> _logger;
        public HttpCall(ILogger<HttpCall> logger)
        {
            _logger = logger;
        }
        public async Task<HttpResponseMessage> PostCall<T>(T request, string ApiUrl, string token) where T : class
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", token);

                    string requestJson = JsonConvert.SerializeObject(request);
                    HttpResponseMessage httpResponse = await client.PostAsync(ApiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

                    return httpResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PostCall metodunda hata alındı.");
                return new HttpResponseMessage();
            }
        }
    }
}
