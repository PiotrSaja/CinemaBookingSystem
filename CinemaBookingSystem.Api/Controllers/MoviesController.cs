﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Movies.Commands.AddMovieVote;
using CinemaBookingSystem.Application.Movies.Commands.AddPreferencesMovie;
using CinemaBookingSystem.Application.Movies.Commands.CreateMovieFromExternalApi;
using CinemaBookingSystem.Application.Movies.Commands.DeleteMovie;
using CinemaBookingSystem.Application.Movies.Commands.UpdateMovie;
using CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail;
using CinemaBookingSystem.Application.Movies.Queries.GetMovies;
using CinemaBookingSystem.Application.Movies.Queries.GetMoviesDaysToPremiere;
using CinemaBookingSystem.Application.Movies.Queries.GetMoviesWithSeanceOnCurrentCinemaAndDay;
using CinemaBookingSystem.Application.Movies.Queries.GetUserMovieVote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/movies")]
    public class MoviesController : BaseController
    {
        private readonly IUserVoteService _userVoteService;
        private readonly IUserService _userService;

        public MoviesController(IUserVoteService userVoteService, IUserService userService)
        {
            _userVoteService = userVoteService;
            _userService = userService;
        }

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
        [HttpPost("omdb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateMovieFromExternalApi(CreateMovieFromExternalApiCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieCommand movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(movie));
        }
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
        [HttpGet("kmeans")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Kmeans()
        {
            await _userVoteService.Clustering(CancellationToken.None);

            return Ok();
        }

        [HttpGet("get-predictions")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> GetPredictions()
        {
            var result = await _userVoteService.GetPredictions(_userService.Id, CancellationToken.None);

            return Ok(result);
        }
        [HttpGet("vote/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult<MovieDetailVm>> GetUserMovieVote(int id)
        {
            var vm = await Mediator.Send(new GetUserMovieVoteQuery() { MovieId = id });

            return Ok(vm);
        }
    }
}
