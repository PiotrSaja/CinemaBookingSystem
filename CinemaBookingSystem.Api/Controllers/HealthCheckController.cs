using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
using CinemaBookingSystem.Infrastructure.ExternalAPI.OMDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/hc")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        public IOmdbClient OmdbClient;

        public HealthCheckController(IOmdbClient omdbClient)
        {
            OmdbClient = omdbClient;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetAsync()
        {
            var movie = await OmdbClient.GetMovieById("tt2382320", CancellationToken.None);

            MovieModel movieModel =
                JsonSerializer.Deserialize<MovieModel>(movie);

            Console.WriteLine(movieModel.Title);

            return "Api healthy";
        }

    }
}
