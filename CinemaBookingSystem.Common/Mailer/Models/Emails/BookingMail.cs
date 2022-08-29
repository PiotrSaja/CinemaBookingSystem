using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Common.Mailer.Models.Emails
{
    public class BookingMail
    {
        public string Tittle { get; set; }
        public string Date { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string BookingId { get; set; }
    }
}
