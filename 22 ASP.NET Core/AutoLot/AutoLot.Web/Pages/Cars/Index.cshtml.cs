namespace AutoLot.Web.Pages.Cars;
public class IndexModel : BasePageModel<Car, IndexModel>
{
    public string MakeName { get; set; }
    public int? MakeId { get; set; }
    public IEnumerable<Car> CarRecords { get; set; }

    public IndexModel(
        IAppLogging<IndexModel> appLogging, 
        ICarDataService dataService) : base(appLogging, dataService, "Cars")
    {
    }

    public async Task OnGetAsync(int? makeId, string makeName)
    {
        MakeId = makeId;
        MakeName = makeName;
        CarRecords = await ((ICarDataService)DataService).GetAllByMakeIdAsync(makeId);
    }
}
