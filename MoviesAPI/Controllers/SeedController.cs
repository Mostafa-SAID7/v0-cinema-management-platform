using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Domain.Entities.Movies;
using MoviesAPI.Domain.Entities.Screenings;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Data;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SeedController(
            IMovieRepository movieRepository,
            IScreeningRepository screeningRepository,
            IHallRepository hallRepository,
            IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _screeningRepository = screeningRepository;
            _hallRepository = hallRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("movies")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 500)]
        public async Task<IActionResult> SeedMovies()
        {
            try
            {
                var movies = new List<Movie>
                {
                    new Movie
                    {
                        Name = "Neon Horizon",
                        Duration = 142,
                        ReleaseDate = new DateTime(2026, 3, 6),
                        Amount = 15.00m,
                        PosterPath = "poster-1",
                        Plot = "In a world where reality and virtual existence collide, one hacker must navigate both dimensions to save humanity from a rogue AI that threatens to merge the two worlds permanently.",
                        Actors = "Marcus Webb, Luna Park, Devon Cross, Zara Knight",
                        Directors = "Aria Chen",
                        Genres = "Sci-Fi,Action,Thriller"
                    },
                    new Movie
                    {
                        Name = "Amber Skies",
                        Duration = 118,
                        ReleaseDate = new DateTime(2026, 3, 6),
                        Amount = 12.00m,
                        PosterPath = "poster-2",
                        Plot = "Two strangers connected by a shared melody find each other across continents, only to discover that their pasts are more intertwined than they ever imagined.",
                        Actors = "Elena Vasquez, James Okonkwo",
                        Directors = "Sofia Morales",
                        Genres = "Romance,Drama"
                    },
                    new Movie
                    {
                        Name = "The Hollow",
                        Duration = 107,
                        ReleaseDate = new DateTime(2026, 3, 6),
                        Amount = 12.00m,
                        PosterPath = "poster-3",
                        Plot = "A family inherits a sprawling Victorian mansion, only to discover its walls hold terrifying secrets that grow stronger with every night they stay.",
                        Actors = "Sarah Mitchell, Rory Graham",
                        Directors = "Nathan Voss",
                        Genres = "Horror,Thriller"
                    },
                    new Movie
                    {
                        Name = "Sky Wanderers",
                        Duration = 102,
                        ReleaseDate = new DateTime(2026, 3, 6),
                        Amount = 13.00m,
                        PosterPath = "poster-4",
                        Plot = "A misfit crew of flying creatures embarks on an epic quest across floating islands to find the legendary Storm Crystal before their world crumbles.",
                        Actors = "Chris Pratt, Awkwafina",
                        Directors = "Yuki Tanaka",
                        Genres = "Animation,Adventure"
                    },
                    new Movie
                    {
                        Name = "Best Laid Plans",
                        Duration = 98,
                        ReleaseDate = new DateTime(2026, 3, 6),
                        Amount = 12.00m,
                        PosterPath = "poster-5",
                        Plot = "Four old college friends reunite for a weekend getaway that spirals into a series of increasingly absurd misadventures involving a stolen yacht and an angry llama.",
                        Actors = "Dave Kim, Amy Pascal",
                        Directors = "Tanya Briggs",
                        Genres = "Comedy"
                    },
                    new Movie
                    {
                        Name = "Iron Resolve",
                        Duration = 155,
                        ReleaseDate = new DateTime(2026, 3, 6),
                        Amount = 18.00m,
                        PosterPath = "poster-6",
                        Plot = "Based on true events, a band of soldiers must hold a critical bridge against overwhelming odds during the final days of a devastating conflict.",
                        Actors = "Tom Hardy, Florence Pugh",
                        Directors = "Robert Jansen",
                        Genres = "Action,Drama"
                    }
                };

                var createdIds = new List<Guid>();
                foreach (var movie in movies)
                {
                    var created = await _movieRepository.CreateMovieAsync(movie);
                    createdIds.Add(created.Id);
                }
                await _unitOfWork.SaveChangesAsync();

                return Ok(BaseResponse<object>.Success(
                    new { movieIds = createdIds },
                    $"Successfully seeded {createdIds.Count} movies"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponse<object>.Failure($"Error seeding movies: {ex.Message}"));
            }
        }

        [HttpPost("screenings")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 500)]
        public async Task<IActionResult> SeedScreenings()
        {
            try
            {
                var movies = await _movieRepository.GetMoviesAsync();
                
                if (!movies.Any())
                    return BadRequest(BaseResponse<object>.Failure("No movies found. Please seed movies first."));

                var halls = await _hallRepository.GetAllHallsAsync();
                if (!halls.Any())
                    return BadRequest(BaseResponse<object>.Failure("No halls found. Please create halls first."));

                var hallIds = halls.Select(h => h.Id).ToList();
                var screenings = new List<Screening>();
                var baseDate = DateTime.Now.Date.AddDays(1);

                foreach (var movie in movies.Take(6))
                {
                    screenings.Add(new Screening
                    {
                        MovieId = movie.Id,
                        ScreeningDateTime = baseDate.AddHours(10),
                        HallId = hallIds[0]
                    });

                    if (hallIds.Count > 1)
                    {
                        screenings.Add(new Screening
                        {
                            MovieId = movie.Id,
                            ScreeningDateTime = baseDate.AddHours(14),
                            HallId = hallIds[1]
                        });
                    }

                    screenings.Add(new Screening
                    {
                        MovieId = movie.Id,
                        ScreeningDateTime = baseDate.AddHours(18),
                        HallId = hallIds[0]
                    });

                    if (hallIds.Count > 2)
                    {
                        screenings.Add(new Screening
                        {
                            MovieId = movie.Id,
                            ScreeningDateTime = baseDate.AddHours(21),
                            HallId = hallIds[2]
                        });
                    }
                }

                var createdIds = new List<Guid>();
                foreach (var screening in screenings)
                {
                    var created = await _screeningRepository.CreateScreeningAsync(screening);
                    createdIds.Add(created.Id);
                }
                await _unitOfWork.SaveChangesAsync();

                return Ok(BaseResponse<object>.Success(
                    new { screeningIds = createdIds },
                    $"Successfully seeded {createdIds.Count} screenings"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponse<object>.Failure($"Error seeding screenings: {ex.Message}"));
            }
        }

        [HttpPost("all")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 500)]
        public async Task<IActionResult> SeedAll()
        {
            try
            {
                var moviesResult = await SeedMovies();
                if (moviesResult is not OkObjectResult)
                    return moviesResult;

                var screeningsResult = await SeedScreenings();
                if (screeningsResult is not OkObjectResult)
                    return screeningsResult;

                return Ok(BaseResponse<object>.Success(null, "Successfully seeded all data (movies and screenings)"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponse<object>.Failure($"Error seeding data: {ex.Message}"));
            }
        }

        [HttpDelete("all")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        public async Task<IActionResult> ClearAll()
        {
            return Ok(BaseResponse<object>.Success(null, "Clear all endpoint - implement based on your needs"));
        }
    }
}
