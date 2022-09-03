using System.Threading.Tasks;
using CinemaBookingSystem.Application.Cinemas.Commands.CreateCinema;
using CinemaBookingSystem.Application.Cinemas.Commands.DeleteCinema;
using CinemaBookingSystem.Application.Cinemas.Commands.UpdateCinema;
using CinemaBookingSystem.Application.Cinemas.Queries.GetCinemaDetail;
using CinemaBookingSystem.Application.Cinemas.Queries.GetCinemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/cinemas")]
    public class CinemasController : BaseController
    {
        #region GetDetails()

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CinemaDetailVm>> GetDetails(int id)
        {
            var vm = await Mediator.Send(new GetCinemaDetailQuery() { CinemaId = id });

            return Ok(vm);
        }

        #endregion

        #region GetCinemas()

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCinemas()
        {
            var vm = await Mediator.Send(new GetCinemasQuery());
            return Ok(vm);
        }

        #endregion

        #region CreateCinema()
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateCinema(CreateCinemaCommand cinema)
        {
            var result = await Mediator.Send(cinema);

            return Ok(result);
        }

        #endregion

        #region DeleteCinema()

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteCinema(int id)
        {
            var result = await Mediator.Send(new DeleteCinemaCommand() { CinemaId = id });
            return Ok(result);
        }

        #endregion

        #region UpdateCinema()

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateCinema(int id, UpdateCinemaCommand cinema)
        {
            if (id != cinema.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(cinema));
        }

        #endregion 
    }
}
