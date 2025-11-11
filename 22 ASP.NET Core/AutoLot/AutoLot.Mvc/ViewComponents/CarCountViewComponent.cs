namespace AutoLot.Mvc.ViewComponents;

public class CarCountViewComponent : ViewComponent
{
    private readonly ICarDataService _dataService;

    public CarCountViewComponent(ICarDataService dataService)
    {
        _dataService = dataService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var count = (await _dataService.GetAllAsync()).Count();
  
        return View("CarCountView", count);
    }

}
