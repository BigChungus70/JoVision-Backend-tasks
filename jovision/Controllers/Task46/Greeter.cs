using Microsoft.AspNetCore.Mvc;

namespace jovision.Controllers.Task46;

[ApiController]
[Route("task46/[controller]")]
public class Greeter : ControllerBase
{


    [HttpPost]
    public string Post([FromForm] string name = "anonymous")
    {
        return $"Hello {name}";
        
    }
}
