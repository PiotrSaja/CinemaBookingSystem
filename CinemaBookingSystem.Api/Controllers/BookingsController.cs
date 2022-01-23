using System.Threading.Tasks;
using CinemaBookingSystem.Application.Bookings.Commands.ChangeStatusOfBooking;
using CinemaBookingSystem.Application.Bookings.Commands.CreateBooking;
using CinemaBookingSystem.Application.Bookings.Commands.DeleteBooking;
using CinemaBookingSystem.Application.Bookings.Queries.GetBookingDetail;
using CinemaBookingSystem.Application.Bookings.Queries.GetBookings;
using CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail;
using CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/bookings")]
    public class BookingsController : BaseController
    {
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetDetails(int id)
        {
            var vm = await Mediator.Send(new GetBookingDetailQuery() { BookingId = id });

            return Ok(vm);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetBookings(int page, int limit, string searchString)
        {
            var vm = await Mediator.Send(new GetBookingsQuery()
            {
                PageIndex = page,
                PageSize = limit,
                SearchString = searchString
            });

            return Ok(vm);
        }
        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult> GetUserBookingDetails(int id)
        {
            var vm = await Mediator.Send(new GetUserBookingDetailQuery() { BookingId = id });

            return Ok(vm);
        }
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult> GetUserBookings()
        {
            var vm = await Mediator.Send(new GetUserBookingsQuery());

            return Ok(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult> CreateBooking(CreateBookingCommand booking)
        {
            var result = await Mediator.Send(booking);

            return Ok(result);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            var result = await Mediator.Send(new DeleteBookingCommand() { BookingId = id });
            return Ok(result);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateBooking(int id, ChangeStatusOfBookingCommand booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(booking));
        }
    }
}
