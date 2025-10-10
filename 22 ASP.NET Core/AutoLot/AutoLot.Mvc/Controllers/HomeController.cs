using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Mvc.Models;
using AutoLot.Services.ApiWrapper.Models;
using AutoLot.Services.DataServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoLot.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppLogging<HomeController> _logger;
        public HomeController(IAppLogging<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
        {
            DealerInfo? vm = dealerMonitor.CurrentValue;
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> RazorSyntaxAsync()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
