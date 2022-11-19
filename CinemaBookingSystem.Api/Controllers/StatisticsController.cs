using System;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Statistics.Queries;
using CinemaBookingSystem.Application.Statistics.Queries.GetDataToLineChart;
using CinemaBookingSystem.Application.Statistics.Queries.GetStatistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/statistics")]
    [Authorize(Roles = "Administrator, User")]
    public class StatisticsController : BaseController
    {
        #region GetStatistics()

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatistics() => Ok(await Mediator.Send(new GetStatisticsQuery()));

        [HttpGet("chart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLineChartData([FromQuery] DateTime? from, [FromQuery] DateTime? to,
            [FromQuery] string type, [FromQuery] int? month)
        {
           return  Ok(await Mediator.Send(new GetDataToLineChartQuery()
           {
               DataType = type,
               DateTimeFrom = from,
               DateTimeTo = to,
               Month = month
           }));
        }
        #endregion
    }
}
