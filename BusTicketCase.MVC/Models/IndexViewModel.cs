using Microsoft.AspNetCore.Mvc.Rendering;
using BusTicketCase.MVC.Validation;

namespace BusTicketCase.MVC.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            Origin = new List<SelectListItem>();
            Destination = new List<SelectListItem>();
        }
        [NotEqual("SelectedDestination", ErrorMessage = "Nereden ve Nereye seçimi aynı olamaz.")]
        public int? SelectedOrigin { get; set; }
        public int? SelectedDestination { get; set; }
        [MinDateToday(ErrorMessage = "Tarih seçimi bugünden küçük olamaz.")]
        public DateTime DepatureDate { get; set; }
        public List<SelectListItem> Origin { get; set; }
        public List<SelectListItem> Destination { get; set; }

    }
}
