using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Bookings.Commands.ChangeStatusOfBooking;
using CinemaBookingSystem.Application.Bookings.Commands.CreateBooking;
using CinemaBookingSystem.Application.Bookings.Commands.DeleteBooking;
using CinemaBookingSystem.Application.Bookings.Queries.GetBookingDetail;
using CinemaBookingSystem.Application.Bookings.Queries.GetBookings;
using CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail;
using CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings;
using CinemaBookingSystem.Common.Mailer.Abstractions;
using CinemaBookingSystem.Common.Mailer.Models;
using CinemaBookingSystem.Common.Mailer.Models.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.Api.Controllers
{
    [Route("api/bookings")]
    public class BookingsController : BaseController
    {
        protected IMailService MailService { get; set; }

        #region BookingsController()
        public BookingsController(IMailService mailService)
        {
            MailService = mailService;
        }
        #endregion

        #region GetDetails()

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetDetails(int id)
        {
            var vm = await Mediator.Send(new GetBookingDetailQuery() { BookingId = id });

            return Ok(vm);
        }
        #endregion

        #region GetBookings()

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

        #endregion

        #region GetUserBookingDetails()

        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult> GetUserBookingDetails(int id)
        {
            var vm = await Mediator.Send(new GetUserBookingDetailQuery() { BookingId = id });

            return Ok(vm);
        }

        #endregion

        #region GetUserBookings()

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult> GetUserBookings()
        {
            var vm = await Mediator.Send(new GetUserBookingsQuery());

            return Ok(vm);
        }

        #endregion

        #region CreateBooking()

        [HttpPost]
        [Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult> CreateBooking(CreateBookingCommand booking)
        {
            var result = await Mediator.Send(booking);

            var bookingMail = new BookingMail()
            {
                BookingId = result.BookingId,
                Date = result.Date,
                Tittle = result.Title,
                Firstname = result.Firstname,
                Surname = result.Surname,
                Email = result.Email
            };

            MailData mailData = new MailData(
                new List<string> { bookingMail.Email },
                "MyCinema - Thank you for booking a seats",
                MailService.GetEmailTemplate("example", bookingMail));
            bool sendResult = await MailService.SendAsync(mailData, new CancellationToken());

            return Ok(result);
        }

        #endregion

        #region DeleteBooking()

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            var result = await Mediator.Send(new DeleteBookingCommand() { BookingId = id });
            return Ok(result);
        }

        #endregion

        #region UpdateBooking()

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

        #endregion
    }
}
