using System.Threading.Tasks;
using CinemaBookingSystem.Application.CinemaHalls.Commands.CreateCinemaHall;
using CinemaBookingSystem.Application.CinemaHalls.Commands.DeleteCinemaHall;
using CinemaBookingSystem.Application.CinemaHalls.Commands.UpdateCinemaHall;
using CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallDetail;
using CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallsInCinema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/cinema-halls")]
    public class CinemaHallsController : BaseController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<CinemaHallDetailVm>> GetDetails(int id)
        {
            var vm = await Mediator.Send(new GetCinemaHallDetailQuery() { CinemaHallId = id });

            return Ok(vm);
        }
        [HttpGet("cinema/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> GetCinemaHalls(int id)
        {
            var vm = await Mediator.Send(new GetCinemaHallsInCinemaQuery() {CinemaId = id});
            return Ok(vm);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateCinemaHall(CreateCinemaHallCommand cinemaHall)
        {
            var result = await Mediator.Send(cinemaHall);

            return Ok(result);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteCinemaHall(int id)
        {
            var result = await Mediator.Send(new DeleteCinemaHallCommand() { CinemaHallId = id });
            return Ok(result);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateCinemaHall(int id, UpdateCinemaHallCommand cinemaHall)
        {
            if (id != cinemaHall.CinemaHallId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(cinemaHall));
        }
    }
}
