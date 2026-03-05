using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IUserRepository _userRepository;

        public SeedController(
            IMovieRepository movieRepository,
            IScreeningRepository screeningRepository,
            IHallRepository hallRepository,
            IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _screeningRepository = screeningRepository;
            _hallRepository = hallRepository;
            _userRepository = userRepository;
        }

        [HttpPost("movies")]
        public async Task<IActionResult> SeedMovies()
        {
            try
            {
                // Seed movies based on the frontend mock data
                var movies = new List<CreateAndUpdateMovie>
                {
                    new CreateAndUpdateMovie
                    {
                        Name = "Neon Horizon",
                        Duration = 142, // 2h 22m
                        Release_Date = new DateTime(2026, 3, 6),
                        Amount = 15.00m,
                        Poster_Path = "poster-1",
                        Plot = "In a world where reality and virtual existence collide, one hacker must navigate both dimensions to save humanity from a rogue AI that threatens to merge the two worlds permanently.",
                        Actors = "Marcus Webb, Luna Park, Devon Cross, Zara Knight",
                        Directors = "Aria Chen",
                        Genres = new List<string> { "Sci-Fi", "Action", "Thriller" }
                    },
                    new CreateAndUpdateMovie
                    {
                        Name = "Amber Skies",
                        Duration = 118, // 1h 58m
                        Release_Date = new DateTime(2026, 3, 6),
                        Amount = 12.00m,
                        Poster_Path = "poster-2",
                        Plot = "Two strangers connected by a shared melody find each other across continents, only to discover that their pasts are more intertwined than they ever imagined.",
                        Actors = "Elena Vasquez, James Okonkwo",
                        Directors = "Sofia Morales",
                        Genres = new List<string> { "Romance", "Drama" }
                    },
                    new CreateAndUpdateMovie
                    {
                        Name = "The Hollow",
                        Duration = 107, // 1h 47m
                        Release_Date = new DateTime(2026, 3, 6),
                        Amount = 12.00m,
                        Poster_Path = "poster-3",
                        Plot = "A family inherits a sprawling Victorian mansion, only to discover its walls hold terrifying secrets that grow stronger with every night they stay.",
                        Actors = "Sarah Mitchell, Rory Graham",
                        Directors = "Nathan Voss",
                        Genres = new List<string> { "Horror", "Thriller" }
                    },
                    new CreateAndUpdateMovie
                    {
                        Name = "Sky Wanderers",
                        Duration = 102, // 1h 42m
                        Release_Date = new DateTime(2026, 3, 6),
                        Amount = 13.00m,
                        Poster_Path = "poster-4",
                        Plot = "A misfit crew of flying creatures embarks on an epic quest across floating islands to find the legendary Storm Crystal before their world crumbles.",
                        Actors = "Chris Pratt, Awkwafina",
                        Directors = "Yuki Tanaka",
                        Genres = new List<string> { "Animation", "Adventure" }
                    },
                    new CreateAndUpdateMovie
                    {
                        Name = "Best Laid Plans",
                        Duration = 98, // 1h 38m
                        Release_Date = new DateTime(2026, 3, 6),
                        Amount = 12.00m,
                        Poster_Path = "poster-5",
                        Plot = "Four old college friends reunite for a weekend getaway that spirals into a series of increasingly absurd misadventures involving a stolen yacht and an angry llama.",
                        Actors = "Dave Kim, Amy Pascal",
                        Directors = "Tanya Briggs",
                        Genres = new List<string> { "Comedy" }
                    },
                    new CreateAndUpdateMovie
                    {
                        Name = "Iron Resolve",
                        Duration = 155, // 2h 35m
                        Release_Date = new DateTime(2026, 3, 6),
                        Amount = 18.00m,
                        Poster_Path = "poster-6",
                        Plot = "Based on true events, a band of soldiers must hold a critical bridge against overwhelming odds during the final days of a devastating conflict.",
                        Actors = "Tom Hardy, Florence Pugh",
                        Directors = "Robert Jansen",
                        Genres = new List<string> { "Action", "Drama" }
                    }
                };

                var createdIds = new List<long>();
                foreach (var movie in movies)
                {
                    var id = await _movieRepository.CreateMovieAsync(movie);
                    createdIds.Add(id);
                }

                return Ok(new
                {
                    message = $"Successfully seeded {createdIds.Count} movies",
                    movieIds = createdIds
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error seeding movies", error = ex.Message });
            }
        }

        [HttpPost("screenings")]
        public async Task<IActionResult> SeedScreenings()
        {
            try
            {
                // Get all movies to create screenings for them
                var movies = await _movieRepository.GetMoviesAsync();
                
                if (!movies.Any())
                {
                    return BadRequest(new { message = "No movies found. Please seed movies first." });
                }

                var screenings = new List<CreateScreening>();
                var baseDate = DateTime.Now.Date.AddDays(1); // Tomorrow

                // Create screenings for each movie
                foreach (var movie in movies.Take(6)) // First 6 movies
                {
                    // Morning screening
                    screenings.Add(new CreateScreening
                    {
                        Movie_Id = (int)movie.Id,
                        Screening_Date_Time = baseDate.AddHours(10),
                        Hall_Id = 1
                    });

                    // Afternoon screening
                    screenings.Add(new CreateScreening
                    {
                        Movie_Id = (int)movie.Id,
                        Screening_Date_Time = baseDate.AddHours(14),
                        Hall_Id = 2
                    });

                    // Evening screening
                    screenings.Add(new CreateScreening
                    {
                        Movie_Id = (int)movie.Id,
                        Screening_Date_Time = baseDate.AddHours(18),
                        Hall_Id = 1
                    });

                    // Night screening
                    screenings.Add(new CreateScreening
                    {
                        Movie_Id = (int)movie.Id,
                        Screening_Date_Time = baseDate.AddHours(21),
                        Hall_Id = 3
                    });
                }

                var createdIds = new List<long>();
                foreach (var screening in screenings)
                {
                    var id = await _screeningRepository.CreateScreeningAsync(screening);
                    createdIds.Add(id);
                }

                return Ok(new
                {
                    message = $"Successfully seeded {createdIds.Count} screenings",
                    screeningIds = createdIds
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error seeding screenings", error = ex.Message });
            }
        }

        [HttpPost("all")]
        public async Task<IActionResult> SeedAll()
        {
            try
            {
                // Seed users first
                var usersResult = await SeedUsers();
                if (usersResult is not OkObjectResult)
                {
                    return usersResult;
                }

                // Seed movies
                var moviesResult = await SeedMovies();
                if (moviesResult is not OkObjectResult)
                {
                    return moviesResult;
                }

                // Then seed screenings
                var screeningsResult = await SeedScreenings();
                if (screeningsResult is not OkObjectResult)
                {
                    return screeningsResult;
                }

                return Ok(new
                {
                    message = "Successfully seeded all data (users, movies and screenings)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error seeding data", error = ex.Message });
            }
        }

        [HttpPost("users")]
        public async Task<IActionResult> SeedUsers()
        {
            try
            {
                // Check if users already exist
                var existingUsers = await _userRepository.GetUsersAsync();
                if (existingUsers.Any())
                {
                    return Ok(new { message = "Users already exist, skipping seed" });
                }

                var users = new List<Models.System.RegisterRequest>
                {
                    new Models.System.RegisterRequest
                    {
                        Name = "Test User",
                        Username = "testuser",
                        Email = "test@shoftv.com",
                        Password = "Test123!",
                        Phone = "+1234567890",
                        isActive = true,
                        EmailConfirmed = true
                    },
                    new Models.System.RegisterRequest
                    {
                        Name = "Admin User",
                        Username = "admin",
                        Email = "admin@shoftv.com",
                        Password = "Admin123!",
                        Phone = "+1234567891",
                        isActive = true,
                        EmailConfirmed = true
                    }
                };

                var createdIds = new List<int>();
                foreach (var user in users)
                {
                    var id = await _userRepository.CreateUserAsync(user);
                    createdIds.Add(id);
                }

                return Ok(new
                {
                    message = $"Successfully seeded {createdIds.Count} users",
                    credentials = new[]
                    {
                        new { username = "testuser", password = "Test123!", email = "test@shoftv.com" },
                        new { username = "admin", password = "Admin123!", email = "admin@shoftv.com" }
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error seeding users", error = ex.Message });
            }
        }

        [HttpDelete("all")]
        public async Task<IActionResult> ClearAll()
        {
            try
            {
                // Note: This is a simple implementation
                // In production, you'd want proper cascade delete or transaction handling
                
                return Ok(new
                {
                    message = "Clear all endpoint - implement based on your needs"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error clearing data", error = ex.Message });
            }
        }
    }
}
