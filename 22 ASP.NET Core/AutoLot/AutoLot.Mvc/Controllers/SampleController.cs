
namespace AutoLot.Mvc.Controllers;

public class SampleController : Controller
{
    [Route("[controller]")]
    public IActionResult SampleAction()
    {
        return View("MyIndex");
    }
}
