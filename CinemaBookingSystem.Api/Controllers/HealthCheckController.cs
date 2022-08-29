using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
using CinemaBookingSystem.Common.Mailer.Abstractions;
using CinemaBookingSystem.Common.Mailer.Models;
using CinemaBookingSystem.Common.Mailer.Models.Emails;
using CinemaBookingSystem.Infrastructure.ExternalAPI.OMDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/hc")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetAsync()
        {
            return "Api healthy";
        }
    }
}
