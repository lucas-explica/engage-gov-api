using System.Threading;
using System.Threading.Tasks;
using EngageGov.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GovernmentController : ControllerBase
    {
        private readonly IGovernmentDataService _gov;

        public GovernmentController(IGovernmentDataService gov)
        {
            _gov = gov;
        }

        [HttpGet("representatives")]
        public async Task<IActionResult> GetRepresentatives([FromQuery] string source = "camara", CancellationToken cancellationToken = default)
        {
            var reps = await _gov.GetRepresentativesAsync(source, cancellationToken);
            return Ok(reps);
        }

        [HttpGet("laws")]
        public async Task<IActionResult> GetLaws([FromQuery] string source = "camara", [FromQuery] int? year = null, [FromQuery] int items = 20, CancellationToken cancellationToken = default)
        {
            var laws = await _gov.GetLawsAsync(source, year, items, cancellationToken);
            return Ok(laws);
        }
    }
}
