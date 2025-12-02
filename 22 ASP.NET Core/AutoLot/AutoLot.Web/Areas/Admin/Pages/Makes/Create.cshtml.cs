namespace AutoLot.Web.Areas.Admin.Pages.Makes;

public class CreateModel : BasePageModel<Make,CreateModel>
{
    public CreateModel(
        IAppLogging<CreateModel> appLogging, 
        IMakeDataService makeService) : base(appLogging, makeService, "Create")
    {
    }
    public async Task<IActionResult> OnPostAsync()
    {
        return await SaveOneAsync(DataService.AddAsync);
    }
}
