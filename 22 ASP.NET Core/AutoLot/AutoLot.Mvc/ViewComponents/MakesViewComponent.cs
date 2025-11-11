namespace AutoLot.Mvc.ViewComponents;

public class MakesViewComponent : ViewComponent
{
    private readonly IMakeDataService _dataService;

    public MakesViewComponent(IMakeDataService dataService)
    {
        _dataService = dataService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var makes = (await _dataService.GetAllAsync()).ToList();
        if (!makes.Any())
        {
            return new ContentViewComponentResult("Unable to get the makes");
        }

        return View("MakesView", makes);
    }
}
