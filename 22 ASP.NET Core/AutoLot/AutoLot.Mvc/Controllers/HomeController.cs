using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Models.Entities;
using AutoLot.Mvc.Models;
using AutoLot.Services.ApiWrapper.Models;
using AutoLot.Services.DataServices.Interfaces;

namespace AutoLot.Mvc.Controllers;

[Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly IAppLogging<HomeController> _logger;

    public List<SelectListItem> Makes { get; } = new List<SelectListItem>
        {
          new SelectListItem { Value = "1", Text = "VW" },
          new SelectListItem { Value = "2", Text = "BMW" },
          new SelectListItem { Value = "3", Text = "ZAZ" },
          new SelectListItem { Value = "4", Text = "Ford" },
        };

    public HomeController(IAppLogging<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("/")]
    [Route("/[controller]")]
    [Route("/[controller]/[action]")]
    public IActionResult Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
    {
        DealerInfo? vm = dealerMonitor.CurrentValue;
        return View(vm);
    }
    [HttpGet]
    public async Task<IActionResult> RazorSyntaxAsync([FromServices] ICarDataService dataService )
    {
        //Car car = new()
        //{
        //    Id = 1,
        //    MakeId = 1,
        //    Color = "Blue",
        //    PetName = "Snoopy",
        //    DateBuilt = DateTime.Now
        //};

        ViewData["LookupValues"] = Makes;
        var car = await dataService.FindAsync(7);
        return View(car);
    }
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    [HttpGet]
    public IActionResult Validation()
    {
        var vm = new AddToCartViewModel
        {
            Id = 1,
            ItemId = 1,
            StockQuantity = 2,
            Quantity = 0
        };
        return View(vm);
    }

    [HttpPost]
    public IActionResult Validation(AddToCartViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        return RedirectToAction(nameof(Validation));
    }

    [HttpGet]
    public IActionResult GrantConsent()
    {
        HttpContext.Features.Get<ITrackingConsentFeature>().GrantConsent();
        return RedirectToAction(nameof(Index), nameof(HomeController).RemoveControllerSuffix(),
            new { area = "" });
    }

    [HttpGet]
    public IActionResult WithdrawConsent()
    {
        HttpContext.Features.Get<ITrackingConsentFeature>().WithdrawConsent();
        return RedirectToAction(nameof(Index), nameof(HomeController).RemoveControllerSuffix(),
            new { area = "" });
    }

}
