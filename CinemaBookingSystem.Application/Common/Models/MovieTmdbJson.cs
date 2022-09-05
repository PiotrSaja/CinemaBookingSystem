using System.Collections.Generic;

namespace CinemaBookingSystem.Application.Common.Models
{
    public class MovieResult
    {
        public string backdrop_path { get; set; }
        public List<int> genre_ids { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string poster_path { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public int id { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public int vote_count { get; set; }
        public string title { get; set; }
        public bool adult { get; set; }
        public double popularity { get; set; }
    }

    public class MovieTmdbJson
    {
        public List<MovieResult> movie_results { get; set; }
        public List<object> person_results { get; set; }
        public List<object> tv_results { get; set; }
        public List<object> tv_episode_results { get; set; }
        public List<object> tv_season_results { get; set; }
    }
}
