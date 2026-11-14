using Microsoft.AspNetCore.Mvc;

namespace SlackJiraAutomation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Status = "Application is running",
            Time = DateTime.UtcNow
        });
    }
}