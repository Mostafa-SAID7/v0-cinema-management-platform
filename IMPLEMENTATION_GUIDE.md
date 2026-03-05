# 🚀 Backend Implementation Guide

## Overview

This guide will help you implement the improved backend architecture with DTOs, Validators, CQRS, and best practices.

## 📦 Step 1: Install Required Packages

```bash
cd Backend/MoviesAPI

# CQRS & Mediator
dotnet add package MediatR --version 12.2.0
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection --version 11.1.0

# Validation
dotnet add package FluentValidation --version 11.9.0
dotnet add package FluentValidation.AspNetCore --version 11.3.0

# Mapping
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

# Caching (Optional)
dotnet add package Microsoft.Extensions.Caching.Memory --version 8.0.0

# Logging (Optional)
dotnet add package Serilog.AspNetCore --version 8.0.0
```

## 📁 Step 2: Create Folder Structure

The following folders have been created:
- ✅ `Application/DTOs/Common/` - Base response wrappers
- ✅ `Application/DTOs/Requests/Movies/` - Request DTOs
- ✅ `Application/DTOs/Responses/Movies/` - Response DTOs
- ✅ `Application/Validators/Movies/` - FluentValidation validators

**Create these additional folders**:
```bash
mkdir -p Application/DTOs/Requests/Tickets
mkdir -p Application/DTOs/Requests/Users
mkdir -p Application/DTOs/Requests/Screenings
mkdir -p Application/DTOs/Responses/Tickets
mkdir -p Application/DTOs/Responses/Users
mkdir -p Application/DTOs/Responses/Screenings
mkdir -p Application/Validators/Tickets
mkdir -p Application/Validators/Users
mkdir -p Application/Validators/Screenings
mkdir -p Application/Mappings
mkdir -p Application/Commands/Movies
mkdir -p Application/Queries/Movies
mkdir -p Middleware
```

## 🔧 Step 3: Configure Services in Program.cs

Add this to your `Program.cs`:

```csharp
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Memory Cache
builder.Services.AddMemoryCache();

// ... rest of your existing configuration

var app = builder.Build();

// Add Error Handling Middleware (create this next)
app.UseMiddleware<ErrorHandlingMiddleware>();

// ... rest of your existing middleware

app.Run();
```

## 🛡️ Step 4: Create Error Handling Middleware

Create `Middleware/ErrorHandlingMiddleware.cs`:

```csharp
using System.Net;
using System.Text.Json;
using FluentValidation;
using MoviesAPI.Application.DTOs.Common;

namespace MoviesAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            BaseResponse<object> response;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    response = BaseResponse<object>.Failure(
                        validationException.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                    break;

                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    response = BaseResponse<object>.Failure("Resource not found");
                    break;

                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    response = BaseResponse<object>.Failure("Unauthorized access");
                    break;

                case ArgumentException argumentException:
                    code = HttpStatusCode.BadRequest;
                    response = BaseResponse<object>.Failure(argumentException.Message);
                    break;

                default:
                    response = BaseResponse<object>.Failure("An error occurred processing your request");
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var result = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(result);
        }
    }
}
```

## 🗺️ Step 5: Create AutoMapper Profiles

Create `Application/Mappings/MovieProfile.cs`:

```csharp
using AutoMapper;
using MoviesAPI.Application.DTOs.Requests.Movies;
using MoviesAPI.Application.DTOs.Responses.Movies;
using MoviesAPI.Models;

namespace MoviesAPI.Application.Mappings
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            // Entity to Response
            CreateMap<Movie, MovieResponse>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Release_Date))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.Genres) ? new List<string>() : src.Genres.Split(',').ToList()))
                .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => 
                    src.Ratings != null ? src.Ratings.Count : 0));

            CreateMap<Movie, MovieSummaryResponse>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Release_Date))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.Genres) ? new List<string>() : src.Genres.Split(',').ToList()));

            // Request to Entity
            CreateMap<CreateMovieRequest, Movie>()
                .ForMember(dest => dest.Release_Date, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.Poster_Path, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(",", src.Genres)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore());

            CreateMap<UpdateMovieRequest, Movie>()
                .ForMember(dest => dest.Release_Date, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.Poster_Path, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(",", src.Genres)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore());

            // CreateAndUpdateMovie to Movie (for backward compatibility)
            CreateMap<CreateAndUpdateMovie, Movie>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(",", src.Genres)));
        }
    }
}
```

## 📝 Step 6: Update Controllers to Use DTOs

Example: Update `MoviesController.cs`:

```csharp
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Movies;
using MoviesAPI.Application.DTOs.Responses.Movies;
using MoviesAPI.Repositories.Interface;

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

        /// <summary>
        /// Get all movies
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<MovieSummaryResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<MovieSummaryResponse>>>> GetMovies()
        {
            var movies = await _movieRepository.GetMoviesAsync();
            var response = _mapper.Map<List<MovieSummaryResponse>>(movies);
            return Ok(BaseResponse<List<MovieSummaryResponse>>.Success(response));
        }

        /// <summary>
        /// Get movie by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<MovieResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<MovieResponse>>> GetMovie(long id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);

            if (movie == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            var response = _mapper.Map<MovieResponse>(movie);
            return Ok(BaseResponse<MovieResponse>.Success(response));
        }

        /// <summary>
        /// Create a new movie
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<long>), 201)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<long>>> CreateMovie([FromBody] CreateMovieRequest request)
        {
            // Validate
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            // Map and create
            var movie = _mapper.Map<Movie>(request);
            var id = await _movieRepository.CreateMovieAsync(movie);

            return CreatedAtAction(
                nameof(GetMovie),
                new { id },
                BaseResponse<long>.Success(id, "Movie created successfully")
            );
        }

        /// <summary>
        /// Update an existing movie
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<bool>>> UpdateMovie(long id, [FromBody] UpdateMovieRequest request)
        {
            // Check if exists
            var existing = await _movieRepository.GetMovieAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            // Validate
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            // Map and update
            var movie = _mapper.Map<Movie>(request);
            movie.Id = id;
            var result = await _movieRepository.UpdateMovieAsync((int)id, movie);

            return Ok(BaseResponse<bool>.Success(result, "Movie updated successfully"));
        }

        /// <summary>
        /// Delete a movie
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteMovie(long id)
        {
            var existing = await _movieRepository.GetMovieAsync(id);
            if (existing == null)
                return NotFound(BaseResponse<object>.Failure("Movie not found"));

            var result = await _movieRepository.DeleteMovieAsync(id);
            return Ok(BaseResponse<bool>.Success(result, "Movie deleted successfully"));
        }
    }
}
```

## ✅ Step 7: Testing

### Test with Swagger
1. Start the application
2. Go to http://localhost:5272/swagger
3. Test each endpoint
4. Verify validation works (try invalid data)
5. Check response format is consistent

### Test Validation
Try creating a movie with invalid data:
```json
{
  "name": "",
  "duration": -1,
  "amount": -100
}
```

Should return:
```json
{
  "succeeded": false,
  "message": "Operation failed",
  "errors": [
    "Movie name is required",
    "Duration must be greater than 0 minutes",
    "Amount cannot be negative"
  ],
  "data": null
}
```

## 📊 Benefits Achieved

### ✅ Consistent API Responses
All endpoints now return the same response format:
```json
{
  "succeeded": true/false,
  "message": "...",
  "errors": [],
  "data": {...}
}
```

### ✅ Input Validation
- Automatic validation using FluentValidation
- Clear error messages
- Prevents invalid data from reaching the database

### ✅ Separation of Concerns
- DTOs separate API contracts from domain models
- Controllers are cleaner and focused
- Easy to change internal models without breaking API

### ✅ Type Safety
- Strong typing throughout
- IntelliSense support
- Compile-time checking

## 🎯 Next Steps

1. ✅ DTOs created for Movies
2. ⏳ Create DTOs for Tickets, Users, Screenings
3. ⏳ Implement CQRS with MediatR
4. ⏳ Add caching
5. ⏳ Add logging with Serilog

## 📚 Additional Resources

- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [AutoMapper Documentation](https://docs.automapper.org/)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---
**Status**: Phase 1 Foundation Complete
**Next**: Implement remaining DTOs and CQRS pattern
