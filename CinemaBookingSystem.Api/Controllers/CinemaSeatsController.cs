using System.Threading.Tasks;
using CinemaBookingSystem.Application.CinemaSeats.Commands.CreateCinemaSeat;
using CinemaBookingSystem.Application.CinemaSeats.Commands.DeleteCinemaSeat;
using CinemaBookingSystem.Application.CinemaSeats.Commands.UpdateCinemaSeat;
using CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeatDetail;
using CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/cinema-seats")]
    public class CinemaSeatsController : BaseController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult> GetDetails(int id)
        {
            var vm = await Mediator.Send(new GetCinemaSeatDetailQuery() { CinemaSeatId = id });

            return Ok(vm);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> GetCinemaSeats()
        {
            var vm = await Mediator.Send(new GetCinemaSeatsQuery());
            return Ok(vm);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateCinemaSeat(CreateCinemaSeatCommand cinemaSeat)
        {
            var result = await Mediator.Send(cinemaSeat);

            return Ok(result);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteCinemaSeat(int id)
        {
            var result = await Mediator.Send(new DeleteCinemaSeatCommand() { CinemaSeatId = id });

            return Ok(result);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateCinemaSeat(int id, UpdateCinemaSeatCommand cinemaSeat)
        {
            if (id != cinemaSeat.CinemaSeatId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(cinemaSeat));
        }
    }
}
