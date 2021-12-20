using System.Threading.Tasks;
using CinemaBookingSystem.Application.Movies.Commands.CreateMovieFromExternalApi;
using CinemaBookingSystem.Application.Movies.Commands.DeleteMovie;
using CinemaBookingSystem.Application.Movies.Commands.UpdateMovie;
using CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail;
using CinemaBookingSystem.Application.Movies.Queries.GetMovies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : BaseController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MovieDetailVm>> GetDetails(int id)
        {
            var vm = await Mediator.Send(new GetMovieDetailQuery() { MovieId = id });

            return Ok(vm);
        }
        [HttpGet(Name = "GetMovies")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMovies([FromQuery] int page, [FromQuery] int limit)
        {
            var vm = await Mediator.Send(new GetMoviesQuery()
            {
                PageIndex = page,
                PageSize = limit
            });
            return Ok(vm);
        }
        [HttpPost("omdb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> CreateMovieFromExternalApi(CreateMovieFromExternalApiCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var result = await Mediator.Send(new DeleteMovieCommand() { MovieId = id });
            return Ok(result);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieCommand movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(movie));
        }
    }
}
