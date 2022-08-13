using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Movie : AuditableEntity
    {
        public string Title { get; set; }
        public DateTime Released { get; set; }
        public int Duration { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        public ICollection<Actor> Actors { get; set; }
        public string Plot { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string PosterPath { get; set; }
        public string ImdbRating { get; set; }
        public string BackgroundImagePath { get; set; }
        public ICollection<Seance> Seances { get; set; }
    }
}
