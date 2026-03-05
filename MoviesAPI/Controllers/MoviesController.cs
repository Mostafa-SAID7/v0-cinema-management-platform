using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Movies;
using MoviesAPI.Application.DTOs.Responses.Movies;
using AutoMapper;
using FluentValidation;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateMovieRequest> _createValidator;
        private readonly IValidator<UpdateMovieRequest> _updateValidator;

        public MoviesController(
            IMovieRepository movieRepository,
            IMapper mapper,
            IValidator<CreateMovieRequest> createValidator,
            IValidator<UpdateMovieRequest> updateValidator)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }


        // GET: api/movies
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<MovieSummaryResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<MovieSummaryResponse>>>> Get()
        {
            var movies = await _movieRepository.GetMoviesAsync();
            var response = _mapper.Map<List<MovieSummaryResponse>>(movies);
            return Ok(BaseResponse<List<MovieSummaryResponse>>.Success(response));
        }

        // GET api/movies/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<MovieResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<MovieResponse>>> Get(long id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);

            if (movie == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            var response = _mapper.Map<MovieResponse>(movie);
            return Ok(BaseResponse<MovieResponse>.Success(response));
        }

        // POST api/movies
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<long>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<long>>> Post([FromBody] CreateMovieRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var movie = _mapper.Map<CreateAndUpdateMovie>(request);
            var id = await _movieRepository.CreateMovieAsync(movie);
            
            return CreatedAtAction(nameof(Get), new { id }, BaseResponse<long>.Success(id, "Movie created successfully"));
        }

        // PUT api/movies/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<bool>>> Put(int id, [FromBody] UpdateMovieRequest request)
        {
            var existing = await _movieRepository.GetMovieForUpdateAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var movie = _mapper.Map<CreateAndUpdateMovie>(request);
            var result = await _movieRepository.UpdateMovieAsync(id, movie);
            
            return Ok(BaseResponse<bool>.Success(result, "Movie updated successfully"));
        }

        // DELETE api/movies/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _movieRepository.GetMovieAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            var result = await _movieRepository.DeleteMovieAsync(id);
            return Ok(BaseResponse<bool>.Success(result, "Movie deleted successfully"));
        }

        // GET: /api/movies/genre/{genreName}
        [HttpGet("genre/{genreName}")]
        [ProducesResponseType(typeof(BaseResponse<List<MovieSummaryResponse>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<List<MovieSummaryResponse>>>> GetMoviesByGenre(string genreName)
        {
            var movies = await _movieRepository.GetMoviesByGenreAsync(genreName);
            if (!movies.Any())
                return NotFound(BaseResponse<object>.Failure($"No movies found for genre '{genreName}'"));

            var response = _mapper.Map<List<MovieSummaryResponse>>(movies);
            return Ok(BaseResponse<List<MovieSummaryResponse>>.Success(response));
        }

        // GET: /api/movies/genres
        [HttpGet("genres")]
        [ProducesResponseType(typeof(BaseResponse<List<string>>), 200)]
        public async Task<ActionResult<BaseResponse<List<string>>>> GetAllGenres()
        {
            var genres = await _movieRepository.GettAllGenresAsync();
            return Ok(BaseResponse<List<string>>.Success(genres));
        }

        // GET: /api/movies/futuremovies
        [HttpGet("futuremovies")]
        [ProducesResponseType(typeof(BaseResponse<List<FutureMovie>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<List<FutureMovie>>>> GetFutureMovies()
        {
            var movies = await _movieRepository.GetFutureMoviesAsync();
            if (!movies.Any())
                return NotFound(BaseResponse<object>.Failure("No future movies found"));

            return Ok(BaseResponse<List<FutureMovie>>.Success(movies));
        }

        // POST api/movies/futuremovies
        [HttpPost("futuremovies")]
        [ProducesResponseType(typeof(BaseResponse<long>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<long>>> Post([FromBody] CreateFutureMovie movie)
        {
            if (!ModelState.IsValid)
                return BadRequest(BaseResponse<object>.Failure(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            var id = await _movieRepository.CreateFutureMovieAsync(movie);
            return Ok(BaseResponse<long>.Success(id, "Future movie created successfully"));
        }

        // DELETE api/movies/futuremovies/5
        [HttpDelete("futuremovies/{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> DeleteFutureMovie(long id)
        {
            var existing = await _movieRepository.GetFutureMovieAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Future movie not found"));

            var result = await _movieRepository.DeleteFutureMovieAsync(id);
            return Ok(BaseResponse<bool>.Success(result, "Future movie deleted successfully"));
        }

        // GET: /api/movies/5/rating/3
        [HttpGet("{idMovie}/rating/{idUser}")]
        [ProducesResponseType(typeof(BaseResponse<int>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<int>>> GetRatingOfUserForMovie(long idMovie, long idUser)
        {
            var rating = await _movieRepository.GetRatingOfUserForMovieAsync(idMovie, idUser);
            if (rating == null)
                return NotFound(BaseResponse<object>.Failure("Rating not found"));

            return Ok(BaseResponse<int>.Success(rating.Value));
        }

        // GET: /api/movies/toprated/4
        [HttpGet("toprated/{n}")]
        [ProducesResponseType(typeof(BaseResponse<List<MovieSummaryResponse>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<List<MovieSummaryResponse>>>> GetTopNRated(int n)
        {
            var movies = await _movieRepository.GetTopNMoviesAsync(n);

            if (movies == null || movies.Count == 0)
                return NotFound(BaseResponse<object>.Failure("No top rated movies found"));

            var response = _mapper.Map<List<MovieSummaryResponse>>(movies);
            return Ok(BaseResponse<List<MovieSummaryResponse>>.Success(response));
        }

        // PUT: /api/movies/5/rating
        [HttpPut("{movieId}/rating")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<object>>> UpdateRating(long movieId, [FromBody] CreateRating rating)
        {
            if (rating.Rating < 1 || rating.Rating > 10)
                return BadRequest(BaseResponse<object>.Failure("Rating must be between 1 and 10"));

            rating.MovieId = movieId;

            var success = await _movieRepository.UpsertRating(rating);
            if (!success)
                return StatusCode(500, BaseResponse<object>.Failure("Failed to save rating"));

            return Ok(BaseResponse<object>.Success(new { success = true, message = "Rating saved successfully", rating }, "Rating saved successfully"));
        }
    }
}
