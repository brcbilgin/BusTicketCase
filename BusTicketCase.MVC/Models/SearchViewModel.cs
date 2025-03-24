using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusTicketCase.MVC.Models
{
    public class SearchViewModel
    {
        public string SelectedOption { get; set; }
        public List<SelectListItem> Options { get; set; }
    }
}
