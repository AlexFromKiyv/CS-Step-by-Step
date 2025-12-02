namespace AutoLot.Web.Areas.Admin.Pages.Makes;

public class IndexModel : BasePageModel<Make,IndexModel>
{
    public IEnumerable<Make> MakeRecords { get; set; }
 
    public IndexModel(
        IAppLogging<IndexModel> appLogging, 
        IMakeDataService makeService) 
        : base(appLogging, makeService, "Makes")
    {
    }
    public async Task OnGetAsync()
    {
        MakeRecords = await DataService.GetAllAsync();
    }
}
