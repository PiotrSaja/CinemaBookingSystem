using System;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Movies.Commands.AddMovieVote;
using CinemaBookingSystem.Application.Movies.Commands.AddPreferencesMovie;
using CinemaBookingSystem.Application.Movies.Commands.ClearPreferencesMovies;
using CinemaBookingSystem.Application.Movies.Commands.CreateMovieFromExternalApi;
using CinemaBookingSystem.Application.Movies.Commands.DeleteMovie;
using CinemaBookingSystem.Application.Movies.Commands.UpdateMovie;
using CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail;
using CinemaBookingSystem.Application.Movies.Queries.GetMovies;
using CinemaBookingSystem.Application.Movies.Queries.GetMoviesContentBasedPrediction;
using CinemaBookingSystem.Application.Movies.Queries.GetMoviesDaysToPremiere;
using CinemaBookingSystem.Application.Movies.Queries.GetMoviesForSelectingFavorite;
using CinemaBookingSystem.Application.Movies.Queries.GetMoviesPrediction;
using CinemaBookingSystem.Application.Movies.Queries.GetMoviesWithSeanceOnCurrentCinemaAndDay;
using CinemaBookingSystem.Application.Movies.Queries.GetPrefMovies;
using CinemaBookingSystem.Application.Movies.Queries.GetUserMovieVote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/movies")]
    public class MoviesController : BaseController
    {
        #region MoviesController()
        public MoviesController()
        {
        }
        #endregion

        #region GetDetails()

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MovieDetailVm>> GetDetails(int id)
        {
            var vm = await Mediator.Send(new GetMovieDetailQuery() { MovieId = id });

            return Ok(vm);
        }

        #endregion

        #region GetMovies()

        [HttpGet(Name = "GetMovies")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMovies([FromQuery] int page, [FromQuery] int limit, string searchString)
        {
            var vm = await Mediator.Send(new GetMoviesQuery()
            {
                PageIndex = page,
                PageSize = limit,
                SearchString = searchString
            });
            return Ok(vm);
        }

        #endregion

        #region GetMoviesWithShowsInCinemaOnGivenDay()

        [HttpGet("{id}/{date}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMoviesWithShowsInCinemaOnGivenDay(int id, DateTime date)
        {
            var vm = await Mediator.Send(new GetMoviesWithSeanceOnCurrentCinemaAndDayQuery()
            {
                CinemaId = id,
                Date = date
            });
            return Ok(vm);
        }

        #endregion

        #region CreateMovieFromExternalApi()

        [HttpPost("omdb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateMovieFromExternalApi(CreateMovieFromExternalApiCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }

        #endregion

        #region DeleteMovie()

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var result = await Mediator.Send(new DeleteMovieCommand() { MovieId = id });
            return Ok(result);
        }

        #endregion

        #region UpdateMovie()

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieCommand movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(movie));
        }

        #endregion

        #region GetMoviesDaysToPremiere()

        [HttpGet("soon/{daysToPremiere}", Name = "GetMoviesDaysToPremiere")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMoviesDaysToPremiere(int page, int limit, int daysToPremiere)
        {
            var vm = await Mediator.Send(new GetMoviesDaysToPremiereQuery()
            {
                PageIndex = page,
                PageSize = limit,
                DaysToPremiere = daysToPremiere
            });
            return Ok(vm);
        }

        #endregion

        #region AddVote()

        [HttpPost("vote")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> AddVote([FromBody] AddMovieVoteCommand moveVote)
        {
            var result = await Mediator.Send(moveVote);

            return Ok(result);
        }

        #endregion

        #region AddPrefMovie()

        [HttpPost("pref")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> AddPrefMovie([FromBody] AddPreferencesMovieCommand movePref)
        {
            var result = await Mediator.Send(movePref);

            return Ok(result);
        }

        #endregion

        #region GetMoviesPredictions()

        [HttpGet("k-means/predictions")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> GetMoviesPredictions(int page, int limit, int selectedMovieId)
        {
            var result = await Mediator.Send(new GetMoviesPredictionQuery() { PageIndex = page, PageSize = limit, SelectedMovieId = selectedMovieId});

            return Ok(result);
        }

        #endregion

        #region GetUserMovieVote()


        [HttpGet("vote/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult<MovieDetailVm>> GetUserMovieVote(int id)
        {
            var vm = await Mediator.Send(new GetUserMovieVoteQuery() { MovieId = id });

            return Ok(vm);
        }

        #endregion

        #region GetPrefMovies()

        [HttpGet("pref")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> GetPrefMovies()
        {
            var result = await Mediator.Send(new GetPrefMoviesQuery());

            return Ok(result);
        }

        #endregion

        #region GetMoviesContentBasedPredictions()

        [HttpGet("content-based/predictions")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> GetMoviesContentBasedPredictions(int page, int limit, int selectedMovieId)
        {
            var result = await Mediator.Send(new GetMoviesContentBasedPredictionQuery() { PageIndex = page, PageSize = limit, SelectedMovieId = selectedMovieId});

            return Ok(result);
        }

        #endregion

        #region GetMoviesForSelectingFavorite()

        [HttpGet("selecting-favorite")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMoviesForSelectingFavorite()
        {
            var vm = await Mediator.Send(new GetMoviesForSelectingFavoriteQuery()
            {
                PageIndex = 1,
                PageSize = 24,
                SearchString = String.Empty
            });
            return Ok(vm);
        }

        #endregion

        #region ClearPrefMovie()

        [HttpPost("pref/clear")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> ClearPrefMovie() =>
            Ok(await Mediator.Send(new ClearPreferencesMoviesCommand()));

        #endregion
    }
}
