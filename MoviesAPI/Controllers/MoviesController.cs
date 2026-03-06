using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Domain.Entities.Movies;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Movies;
using MoviesAPI.Application.DTOs.Responses.Movies;
using MoviesAPI.Data;
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
        private readonly IUnitOfWork _unitOfWork;

        public MoviesController(
            IMovieRepository movieRepository,
            IMapper mapper,
            IValidator<CreateMovieRequest> createValidator,
            IValidator<UpdateMovieRequest> updateValidator,
            IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _unitOfWork = unitOfWork;
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
        public async Task<ActionResult<BaseResponse<MovieResponse>>> Get(Guid id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);

            if (movie == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            var response = _mapper.Map<MovieResponse>(movie);
            return Ok(BaseResponse<MovieResponse>.Success(response));
        }

        // POST api/movies
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<Guid>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<Guid>>> Post([FromBody] CreateMovieRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var movie = _mapper.Map<Movie>(request);
            var created = await _movieRepository.CreateMovieAsync(movie);
            await _unitOfWork.SaveChangesAsync();
            
            return CreatedAtAction(nameof(Get), new { id = created.Id }, BaseResponse<Guid>.Success(created.Id, "Movie created successfully"));
        }

        // PUT api/movies/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<object>>> Put(Guid id, [FromBody] UpdateMovieRequest request)
        {
            var existing = await _movieRepository.GetMovieAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            _mapper.Map(request, existing);
            await _movieRepository.UpdateMovieAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            
            return Ok(BaseResponse<object>.Success(null, "Movie updated successfully"));
        }

        // DELETE api/movies/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _movieRepository.GetMovieAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            await _movieRepository.DeleteMovieAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(null, "Movie deleted successfully"));
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
            var genres = await _movieRepository.GetAllGenresAsync();
            return Ok(BaseResponse<List<string>>.Success(genres));
        }

        // GET: /api/movies/5/rating/3
        [HttpGet("{idMovie}/rating/{idUser}")]
        [ProducesResponseType(typeof(BaseResponse<int>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<int>>> GetRatingOfUserForMovie(Guid idMovie, Guid idUser)
        {
            var rating = await _movieRepository.GetRatingOfUserForMovieAsync(idMovie, idUser);
            if (rating == null)
                return NotFound(BaseResponse<object>.Failure("Rating not found"));

            return Ok(BaseResponse<int>.Success(rating.Rating));
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
        public async Task<ActionResult<BaseResponse<object>>> UpdateRating(Guid movieId, [FromBody] CreateRatingRequest request)
        {
            if (request.Rating < 1 || request.Rating > 10)
                return BadRequest(BaseResponse<object>.Failure("Rating must be between 1 and 10"));

            var rating = new MovieRating
            {
                MovieId = movieId,
                UserId = Guid.Parse(request.UserId.ToString()),
                Rating = request.Rating
            };

            var saved = await _movieRepository.UpsertRatingAsync(rating);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(new { success = true, message = "Rating saved successfully", rating = request.Rating }, "Rating saved successfully"));
        }
    }
}
