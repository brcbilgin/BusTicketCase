using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using BusTicketCase.Application.Common;
using BusTicketCase.Application.Common.Cache;
using BusTicketCase.Application.Services.BusService;
using BusTicketCase.Application.Services.BusService.Request;
using BusTicketCase.Application.Services.BusService.Response;
using BusTicketCase.Application.Services.SessionService;
using BusTicketCase.Application.Services.SessionService.Response;
using BusTicketCase.Application.Services.SessionServices;
using BusTicketCase.MVC.Business.Interface;
using BusTicketCase.MVC.Models;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Linq;

namespace BusTicketCase.MVC.Business
{
    public class HomeBusiness : IHomeBusiness
    {
        private readonly ICacheManager _cacheManager;
        private readonly ISessionService _iSessionService;
        private readonly IBusService _obiletService;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeBusiness> _logger;
        public HomeBusiness(ICacheManager cacheManager, ISessionService iSessionService, IBusService obiletService, IMapper mapper, ILogger<HomeBusiness> logger)
        {
            _cacheManager = cacheManager;
            _iSessionService = iSessionService;
            _obiletService = obiletService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IndexViewModel> GetBusLocations(bool isReturnPage)
        {
            IndexViewModel viewModel = new IndexViewModel();
            viewModel.Origin = await _cacheManager.Get<List<SelectListItem>>("origion");
            viewModel.Destination = await _cacheManager.Get<List<SelectListItem>>("destination");

            if (viewModel.Origin == null || viewModel.Destination == null)
            {
                await GetLocations(viewModel);
            }

            return await UpdateIndexViewModel(viewModel, isReturnPage);
        }
        public async Task<JourneyIndexViewModel> GetJourney(IndexViewModel model)
        {
            SesionData deviceSession = await GetSession() ?? new SesionData();
            if (deviceSession != null)
            {
                var response = await _obiletService.GetBusJourneys(new GetJourneysRequest
                {
                    DeviceSession = deviceSession,
                    Data = new Data { DepartureDate = model.DepatureDate, DestinationId = model.SelectedDestination.Value, OriginId = model.SelectedOrigin.Value }
                });

                JourneyIndexViewModel viewModel = _mapper.Map<JourneyIndexViewModel>(response);

                await SetJourneyViewHeader(viewModel, model);

                return viewModel;
            }

            return new JourneyIndexViewModel();
        }
        public async Task SetJourneyDetailCache(IndexViewModel model)
        {
            await _cacheManager.Set("selectedOrigion", model.SelectedOrigin);
            await _cacheManager.Set("selectedDestination", model.SelectedDestination);
            await _cacheManager.Set("selectedDate", model.DepatureDate);
        }
        private async Task<IndexViewModel> UpdateIndexViewModel(IndexViewModel viewModel, bool isReturnPage)
        {
            if (isReturnPage)
            {
                viewModel.SelectedOrigin = await _cacheManager.Get<int>("selectedOrigion");
                viewModel.SelectedDestination = await _cacheManager.Get<int>("selectedDestination");
                viewModel.DepatureDate = await _cacheManager.Get<DateTime>("selectedDate");
            }
            else
            {
                viewModel.DepatureDate = DateTime.Now.AddDays(1);
            }

            return viewModel;
        }
        private async Task<SesionData> GetSession()
        {
            SesionData result = await _cacheManager.Get<SesionData>("session");

            if (result != null) return result;
            
            GetSessionResponse session = await _iSessionService.GetSesion();

            if (session.Status == "Success")
            {
                result = new SesionData();

                result.DeviceId = session.Data.DeviceId;
                result.SessionId = session.Data.SessionId;
            }

            return result;
        }
        private async Task GetLocations(IndexViewModel viewModel)
        {
            SesionData deviceSession = await GetSession() ?? new SesionData();
            if (deviceSession != null)
            {
                GetLocationsResponse response = await _obiletService.GetBusLocations(new GetLocationsRequest
                {
                    DeviceSession = deviceSession
                });

                if (response?.Data != null)
                {
                    var sortedLocations = response.Data.OrderBy(x => x.Name).ToList();

                    List<SelectListItem> origion = await GetLocaitonOrigion(sortedLocations);
                    List<SelectListItem> destination = await GetLocaitonDestination(sortedLocations);

                    await _cacheManager.Set("origion", origion);
                    await _cacheManager.Set("destination", destination);

                    viewModel.Origin = origion;
                    viewModel.Destination = destination;
                }
            }
        }
        private async Task SetJourneyViewHeader(JourneyIndexViewModel viewModel, IndexViewModel model)
        {
            List<SelectListItem> originLocation = await _cacheManager.Get<List<SelectListItem>>("origion");
            if (originLocation != null)
            {
                viewModel.OriginLocation = originLocation.Find(item => item.Value == model.SelectedOrigin.ToString())?.Text;
            }

            List<SelectListItem> destinationLocation = await _cacheManager.Get<List<SelectListItem>>("destination");
            if (destinationLocation != null)
            {
                viewModel.DestinationLocation = destinationLocation.Find(item => item.Value == model.SelectedDestination.ToString())?.Text;
            }

            viewModel.SelectedDate = model.DepatureDate.ToString("d MMMM dddd", CultureInfo.CreateSpecificCulture("tr-TR"));
        }
        private async Task<List<SelectListItem>> GetLocaitonOrigion(List<LocationData> data)
        {
            if (data != null)
            {
                return data.Select(s => new SelectListItem() 
                { 
                    Text = s.Name, 
                    Value = s.Id.ToString(), 
                    Selected = s.Name.Contains("İstanbul") 
                }).ToList();
            }
            return null;
        }
        private async Task<List<SelectListItem>> GetLocaitonDestination(List<LocationData> data)
        {
            if (data != null)
            {
                return data.Select(s => new SelectListItem() 
                { 
                    Text = s.Name, 
                    Value = s.Id.ToString(), 
                    Selected = s.Name.Equals("Ankara") 
                }).ToList();
            }
            return null;
        }
        public async Task GetBrowserInformation(string userAgent)
        {
            try
            {
                BrowserData browserData = await _cacheManager.Get<BrowserData>("browser");

                if (browserData != null) return;

                if (string.IsNullOrEmpty(userAgent)) return;

                string[] userAgentInfo = userAgent.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (userAgentInfo.Length >= 3)
                {
                    browserData = new BrowserData();

                    browserData.Name = userAgentInfo[2];

                    if (userAgentInfo.Length >= 4)
                    {
                        browserData.Version = userAgentInfo[3];
                    }
                }

                await _cacheManager.Set("browser", browserData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBrowserInformation metodunda hata alındı.");
            }
        }
    }
}
