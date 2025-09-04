using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;

namespace AutoLot.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DealerInfo DealerInfoInstance { get; set; }  

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet([FromServices] IOptionsMonitor<DealerInfo> dealerOptions)
        {
            DealerInfoInstance = dealerOptions.CurrentValue;
            //_logger.LogWarning(User.);
        }
    }
}
