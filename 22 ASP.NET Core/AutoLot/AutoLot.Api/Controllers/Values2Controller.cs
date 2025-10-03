namespace AutoLot.Api.Controllers;


[ApiVersion("2.0")]
[ApiVersion("2.0-Beta")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class Values2Controller : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult Get(int id, ApiVersion versionFromModelBinding)
    {
        var versionFromContext = HttpContext.GetRequestedApiVersion();

        return Ok(new[] { versionFromContext.ToString(), versionFromModelBinding.ToString(), id.ToString() });
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] { "Version2:value1", "Version2:value2" });
    }

}
