using System.Threading.Tasks;
using CinemaBookingSystem.Application.Statistics.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/statistics")]
    [Authorize(Roles = "Administrator, User")]
    public class StatisticsController : BaseController
    {
        #region GetStatistics()

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatistics() => Ok(await Mediator.Send(new GetStatisticsQuery()));

        #endregion
    }
}
