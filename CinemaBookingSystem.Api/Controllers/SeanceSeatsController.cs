using System.Threading.Tasks;
using CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeat;
using CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeats;
using CinemaBookingSystem.Application.SeanceSeats.Commands.DeleteSeanceSeat;
using CinemaBookingSystem.Application.SeanceSeats.Commands.LockSeanceSeat;
using CinemaBookingSystem.Application.SeanceSeats.Commands.UpdateSeanceSeat;
using CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatDetail;
using CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatsForSeance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/seance-seats")]
    public class SeanceSeatsController : BaseController
    {
        #region GetDetail()

        [HttpGet("detail/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var vm = await Mediator.Send(new GetSeanceSeatDetailQuery()
            {
                SeanceSeatId = id
            });
            return Ok(vm);
        }

        #endregion

        #region GetAvailableSeanceSeats()

        [HttpGet("{seanceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAvailableSeanceSeats(int seanceId)
        {
            var vm = await Mediator.Send(new GetSeanceSeatsForSeanceQuery()
            {
                SeanceId = seanceId
            });
            return Ok(vm);
        }

        #endregion

        #region CreateSeanceSeat()

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateSeanceSeat(CreateSeanceSeatCommand seanceSeat)
        {
            var result = await Mediator.Send(seanceSeat);

            return Ok(result);
        }

        #endregion

        #region DeleteSeanceSeat()

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteSeanceSeat(int id)
        {
            var result = await Mediator.Send(new DeleteSeanceSeatCommand() { SeanceSeatId = id });

            return Ok(result);
        }

        #endregion

        #region UpdateSeanceSeat()

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateSeanceSeat(int id, UpdateSeanceSeatCommand seanceSeat)
        {
            if (id != seanceSeat.SeanceSeatId)
            {
                return BadRequest();
            }
            var result = await Mediator.Send(seanceSeat);

            return Ok(result);
        }

        #endregion

        #region LockSeanceSeat()

        [HttpPost("seat-lock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult> LockSeanceSeat(LockSeanceSeatCommand lockSeanceSeat)
        {
            var result = await Mediator.Send(lockSeanceSeat);

            return Ok(result);
        }

        #endregion

        #region CreateCinemaSeats()

        [HttpPost("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateCinemaSeats(CreateSeanceSeatsCommand seanceSeats)
        {
            var result = await Mediator.Send(seanceSeats);

            return Ok(result);
        }

        #endregion
    }
}
