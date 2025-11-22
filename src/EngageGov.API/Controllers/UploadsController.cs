using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UploadsController : ControllerBase
{
    private readonly ILogger<UploadsController> _logger;

    public UploadsController(ILogger<UploadsController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public IActionResult Upload()
    {
        // Accepts multipart/form-data; for now return a placeholder URL
        _logger.LogInformation("Received upload request");
        var url = "https://example.com/uploads/placeholder.png";
        return Ok(new { url });
    }
}
