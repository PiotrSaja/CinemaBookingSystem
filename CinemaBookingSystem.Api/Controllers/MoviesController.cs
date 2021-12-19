using System.Threading.Tasks;
using CinemaBookingSystem.Application.Movies.Commands.CreateMovieFromExternalApi;
using CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail;
using CinemaBookingSystem.Application.Movies.Queries.GetMovies;
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
        public async Task<IActionResult> GetMovies()
        {
            var vm = await Mediator.Send(new GetMoviesQuery()
            {
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
    }
}
