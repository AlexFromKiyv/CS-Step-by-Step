namespace AutoLot.Web.Pages;
public class RazorSyntaxModel : PageModel
{
    private readonly ICarDataService _carDataService;

    [BindProperty]
    public Car Entity { get; set; }

    [ViewData]
    public string Title => "Razor Syntax";

    public RazorSyntaxModel(ICarDataService carDataService)
    {
        _carDataService = carDataService;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        Entity = await _carDataService.FindAsync(7);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _carDataService.UpdateAsync(Entity);
        return RedirectToPage("Index");
    }

    //public async Task<IActionResult> OnPostAsync()
    //{
    //    var newCar = new Car();
    //    if (await TryUpdateModelAsync(newCar, "Entity",
    //        c => c.Id,
    //        c => c.TimeStamp,
    //        c => c.PetName,
    //        c => c.Color,
    //        c => c.IsDrivable,
    //        c => c.MakeId,
    //        c => c.Price
    //      ))
    //    {
    //        //do something interesting
    //    }
    //    return RedirectToPage("Index");
    //}

}
