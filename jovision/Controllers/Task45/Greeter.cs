using Microsoft.AspNetCore.Mvc;

namespace jovision.Controllers.Task45;

[ApiController]
[Route("task45/[controller]")]
public class Greeter : ControllerBase
{

    private readonly ILogger<Greeter> _logger;

    public Greeter(ILogger<Greeter> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get([FromQuery] string name = "anonymous")
    {
        return $"Hello {name}";
        
    }
}
