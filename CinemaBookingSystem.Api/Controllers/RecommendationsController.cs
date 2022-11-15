using System;
using CinemaBookingSystem.Application.Recommendations.Commands.ChangeUserRecommendationType;
using CinemaBookingSystem.Application.Recommendations.Queries.WhichRecommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/recommendations")]
    [Authorize(Roles = "Administrator, User")]
    public class RecommendationsController : BaseController
    {
        #region GetRecommendationType()

        [HttpGet("type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecommendationType() => Ok(await Mediator.Send(new WhichRecommendationQuery()));

        #endregion 

        #region UpdateRecommendationType()

        [HttpPost("type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRecommendationType([FromBody] ChangeUserRecommendationTypeCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        } 

        #endregion 
    }
}
