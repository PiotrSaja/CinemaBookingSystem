using System.Threading.Tasks;
using CinemaBookingSystem.Application.Recommendations.Queries.WhichRecommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/recommendations")]
    [Authorize(Roles = "Administrator, User")]
    public class RecommendationsController : BaseController
    {
        [HttpGet("type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecommendationType() => Ok(await Mediator.Send(new WhichRecommendationQuery()));
    }
}
