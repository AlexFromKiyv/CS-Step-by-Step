

namespace AutoLot.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/[controller]/[action]")]
public class MakesController : BaseCrudController<Make, MakesController>
{
    public MakesController(IAppLogging<MakesController> appLogging, IMakeDataService mainDataService) : base(appLogging, mainDataService)
    {
    }

    protected override async Task<SelectList> GetLookupValuesAsync()
        => await Task.FromResult<SelectList>(null);

    [Route("/Admin")]
    [Route("/Admin/[controller]")]
    [Route("/Admin/[controller]/[action]")]
    public override async Task<IActionResult> IndexAsync()
    {
        return await base.IndexAsync();
    }
}
