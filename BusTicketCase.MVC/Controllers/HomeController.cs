using Microsoft.AspNetCore.Mvc;
using BusTicketCase.MVC.Business.Interface;
using BusTicketCase.MVC.Models;
using System.Diagnostics;

namespace BusTicketCase.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeBusiness _homeBusiness;
        public HomeController(IHomeBusiness homeBusiness)
        {
            _homeBusiness = homeBusiness;
        }
        public async Task<IActionResult> Index(bool isReturnPage)
        {
            string userAgent = Request.Headers["User-Agent"].ToString();
            if (string.IsNullOrEmpty(userAgent)) return View("Error");

            await _homeBusiness.GetBrowserInformation(userAgent);

            IndexViewModel viewModel = await _homeBusiness.GetBusLocations(isReturnPage);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetJourneys(IndexViewModel model)
        {
            await _homeBusiness.SetJourneyDetailCache(model);

            if (ModelState.IsValid)
            {
                JourneyIndexViewModel viewModel = await _homeBusiness.GetJourney(model);
                return View("JourneyIndex", viewModel);
            }

            else
            {
                IndexViewModel viewModel = await _homeBusiness.GetBusLocations(true);
                return View("Index", viewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}