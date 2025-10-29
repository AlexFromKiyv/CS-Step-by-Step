namespace AutoLot.Mvc.Controllers;

public class TestBaseCrudController : BaseCrudWithBindingPropertyController<Car, TestBaseCrudController>
{

    private readonly IMakeDataService _lookupDataService;
     
    public TestBaseCrudController(IAppLogging<TestBaseCrudController> appLogging, ICarDataService mainDataService, IMakeDataService makeDataService) : base(appLogging, mainDataService)
    {
        _lookupDataService = makeDataService;
    }

    protected override async Task<SelectList> GetLookupValuesAsync()
        => new SelectList( await _lookupDataService.GetAllAsync(),nameof(Make.Id),nameof(Make.Name));

}
