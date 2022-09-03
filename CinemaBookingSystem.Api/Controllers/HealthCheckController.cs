using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/hc")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        #region GetAsync()

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetAsync()
        {
            return "Api healthy";
        }

        #endregion
    }
}
