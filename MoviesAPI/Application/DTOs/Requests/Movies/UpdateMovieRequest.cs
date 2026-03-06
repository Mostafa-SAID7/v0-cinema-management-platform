using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Application.DTOs.Requests.Movies
{
    public class UpdateMovieRequest
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int Duration { get; set; }

        public string Director { get; set; }

        public string Cast { get; set; }

        public string PosterUrl { get; set; }

        public string TrailerUrl { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}
