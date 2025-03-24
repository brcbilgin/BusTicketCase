using AutoMapper;
using BusTicketCase.Application.Services.BusService.Request;
using BusTicketCase.Application.Services.BusService.Response;
using BusTicketCase.MVC.Models;

namespace BusTicketCase.MVC.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Journey, JourneyModel>()
                .ForMember(dest => dest.Arrival, opt => opt.MapFrom(src => src.Arrival.ToString("HH:mm")))
                .ForMember(dest => dest.Departure, opt => opt.MapFrom(src => src.Departure.ToString("HH:mm")))
                .ReverseMap();
            CreateMap<GetJourneysResponse, JourneyIndexViewModel>()
                .ForMember(dest => dest.Journeys, opt => opt.MapFrom(src => src.Data.Select(x => x.Journey).ToList()))
                .ReverseMap();

        }
    }
}
