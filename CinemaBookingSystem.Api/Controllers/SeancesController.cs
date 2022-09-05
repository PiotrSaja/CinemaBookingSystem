using System;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Seances.Commands.CreateSeance;
using CinemaBookingSystem.Application.Seances.Commands.DeleteSeance;
using CinemaBookingSystem.Application.Seances.Commands.UpdateSeance;
using CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail;
using CinemaBookingSystem.Application.Seances.Queries.GetSeances;
using CinemaBookingSystem.Application.Seances.Queries.GetSeancesOfCurrentMovieOnGivenCinemaAndDay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeanceDetailVm = CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail.SeanceDetailVm;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/seances")]
    public class SeancesController : BaseController
    {
        #region GetDetails()

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SeanceDetailVm>> GetDetails(int id)
        {
            var vm = await Mediator.Send(new GetSeanceDetailQuery() { SeanceId = id });

            return Ok(vm);
        }

        #endregion

        #region GetSeances()

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSeances(int page, int limit, string searchString)
        {
            var vm = await Mediator.Send(new GetSeancesQuery()
            {
                PageIndex = page,
                PageSize = limit,
                SearchString = searchString
            });

            return Ok(vm);
        }

        #endregion

        #region CreateSeance()

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateSeance(CreateSeanceCommand seance)
        {
            var result = await Mediator.Send(seance);

            return Ok(result);
        }

        #endregion

        #region DeleteSeance()

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteSeance(int id)
        {
            var result = await Mediator.Send(new DeleteSeanceCommand() { SeanceId = id });

            return Ok(result);
        }

        #endregion

        #region UpdateSeance()

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateSeance(int id, UpdateSeanceCommand seance)
        {
            if (id != seance.SeanceId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(seance));
        }

        #endregion

        #region GetShowsOfCurrentMovieOnGivenCinemaAndDate()

        [HttpGet("{movieId}/{cinemaId}/{date}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetShowsOfCurrentMovieOnGivenCinemaAndDate(int cinemaId, DateTime date, int movieId)
        {
            var vm = await Mediator.Send(new GetSeancesOfCurrentMovieOnGivenCinemaAndDateQuery()
            {
                CinemaId = cinemaId,
                Date = date,
                MovieId = movieId
            });
            return Ok(vm);
        }

        #endregion 
    }
}
