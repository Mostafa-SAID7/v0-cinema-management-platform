# 🏗️ Backend Architecture Improvement Plan

## Current State Analysis

### ✅ What's Good
- Repository pattern implemented
- Service layer exists
- JWT authentication configured
- Dependency injection setup
- Basic separation of concerns

### ⚠️ What Needs Improvement
- No DTOs (using domain models directly in API)
- No validation attributes
- No CQRS pattern (Commands/Queries)
- No FluentValidation
- No AutoMapper for object mapping
- No proper error handling middleware
- No API versioning
- No response wrappers
- Inconsistent naming conventions (snake_case vs PascalCase)
- No unit of work pattern
- No caching strategy

## 🎯 Improvement Roadmap

### Phase 1: Foundation (High Priority)
1. ✅ Add DTOs for all entities
2. ✅ Implement FluentValidation
3. ✅ Add AutoMapper
4. ✅ Create response wrappers
5. ✅ Add global error handling middleware

### Phase 2: Architecture (Medium Priority)
6. ✅ Implement CQRS with MediatR
7. ✅ Add Unit of Work pattern
8. ✅ Implement specification pattern
9. ✅ Add caching with Redis/Memory Cache
10. ✅ API versioning

### Phase 3: Advanced (Low Priority)
11. ⏳ Add logging with Serilog
12. ⏳ Implement health checks
13. ⏳ Add API rate limiting
14. ⏳ Implement background jobs with Hangfire
15. ⏳ Add API documentation with Swagger annotations

## 📁 New Folder Structure

```
MoviesAPI/
├── Controllers/              # API endpoints
├── Application/              # NEW: Application layer
│   ├── DTOs/                # Data Transfer Objects
│   │   ├── Requests/        # Request DTOs
│   │   ├── Responses/       # Response DTOs
│   │   └── Common/          # Shared DTOs
│   ├── Commands/            # CQRS Commands
│   │   ├── Movies/
│   │   ├── Tickets/
│   │   └── Users/
│   ├── Queries/             # CQRS Queries
│   │   ├── Movies/
│   │   ├── Tickets/
│   │   └── Users/
│   ├── Validators/          # FluentValidation validators
│   ├── Mappings/            # AutoMapper profiles
│   └── Interfaces/          # Application interfaces
├── Domain/                   # NEW: Domain layer
│   ├── Entities/            # Domain entities (moved from Models)
│   ├── Enums/               # Enumerations
│   ├── Exceptions/          # Domain exceptions
│   └── Specifications/      # Specification pattern
├── Infrastructure/           # NEW: Infrastructure layer
│   ├── Persistence/         # Database context, repositories
│   ├── Services/            # External services
│   ├── Caching/             # Caching implementation
│   └── Identity/            # Authentication/Authorization
├── Shared/                   # NEW: Shared layer
│   ├── Constants/           # Application constants
│   ├── Extensions/          # Extension methods
│   ├── Helpers/             # Helper classes
│   └── Wrappers/            # Response wrappers
└── Middleware/               # NEW: Custom middleware
    ├── ErrorHandlingMiddleware.cs
    ├── LoggingMiddleware.cs
    └── PerformanceMiddleware.cs
```

## 🔧 Implementation Details

### 1. DTOs (Data Transfer Objects)

**Purpose**: Separate API contracts from domain models

**Example Structure**:
```csharp
// Application/DTOs/Requests/Movies/CreateMovieRequest.cs
public class CreateMovieRequest
{
    public string Name { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Amount { get; set; }
    public string PosterPath { get; set; }
    public string Plot { get; set; }
    public string Actors { get; set; }
    public string Directors { get; set; }
    public List<string> Genres { get; set; }
}

// Application/DTOs/Responses/Movies/MovieResponse.cs
public class MovieResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Amount { get; set; }
    public string PosterPath { get; set; }
    public string Plot { get; set; }
    public string Actors { get; set; }
    public string Directors { get; set; }
    public List<string> Genres { get; set; }
    public decimal Rating { get; set; }
}
```

### 2. FluentValidation

**Purpose**: Robust input validation

**Example**:
```csharp
// Application/Validators/CreateMovieRequestValidator.cs
public class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Movie name is required")
            .MaximumLength(200).WithMessage("Movie name cannot exceed 200 characters");

        RuleFor(x => x.Duration)
            .GreaterThan(0).WithMessage("Duration must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount cannot be negative");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required");

        RuleFor(x => x.Genres)
            .NotEmpty().WithMessage("At least one genre is required");
    }
}
```

### 3. CQRS with MediatR

**Purpose**: Separate read and write operations

**Commands** (Write operations):
```csharp
// Application/Commands/Movies/CreateMovieCommand.cs
public class CreateMovieCommand : IRequest<Result<long>>
{
    public string Name { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Amount { get; set; }
    public string PosterPath { get; set; }
    public string Plot { get; set; }
    public string Actors { get; set; }
    public string Directors { get; set; }
    public List<string> Genres { get; set; }
}

// Application/Commands/Movies/CreateMovieCommandHandler.cs
public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Result<long>>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public CreateMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<Result<long>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = _mapper.Map<Movie>(request);
        var id = await _movieRepository.CreateMovieAsync(movie);
        return Result<long>.Success(id);
    }
}
```

**Queries** (Read operations):
```csharp
// Application/Queries/Movies/GetMovieByIdQuery.cs
public class GetMovieByIdQuery : IRequest<Result<MovieResponse>>
{
    public long Id { get; set; }
}

// Application/Queries/Movies/GetMovieByIdQueryHandler.cs
public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Result<MovieResponse>>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public GetMovieByIdQueryHandler(IMovieRepository movieRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<Result<MovieResponse>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetMovieAsync(request.Id);
        
        if (movie == null)
            return Result<MovieResponse>.Failure("Movie not found");

        var response = _mapper.Map<MovieResponse>(movie);
        return Result<MovieResponse>.Success(response);
    }
}
```

### 4. Response Wrappers

**Purpose**: Consistent API responses

```csharp
// Shared/Wrappers/Result.cs
public class Result<T>
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public T Data { get; set; }

    public static Result<T> Success(T data, string message = null)
    {
        return new Result<T>
        {
            Succeeded = true,
            Data = data,
            Message = message
        };
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>
        {
            Succeeded = false,
            Errors = new List<string> { error }
        };
    }

    public static Result<T> Failure(List<string> errors)
    {
        return new Result<T>
        {
            Succeeded = false,
            Errors = errors
        };
    }
}

// Shared/Wrappers/PagedResult.cs
public class PagedResult<T> : Result<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}
```

### 5. AutoMapper Profiles

**Purpose**: Object-to-object mapping

```csharp
// Application/Mappings/MovieProfile.cs
public class MovieProfile : Profile
{
    public MovieProfile()
    {
        // Entity to Response
        CreateMap<Movie, MovieResponse>()
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Release_Date));

        // Request to Entity
        CreateMap<CreateMovieRequest, Movie>()
            .ForMember(dest => dest.Release_Date, opt => opt.MapFrom(src => src.ReleaseDate))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(",", src.Genres)));

        // Command to Entity
        CreateMap<CreateMovieCommand, Movie>();
    }
}
```

### 6. Global Error Handling

**Purpose**: Centralized exception handling

```csharp
// Middleware/ErrorHandlingMiddleware.cs
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
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new
                {
                    succeeded = false,
                    errors = validationException.Errors.Select(e => e.ErrorMessage)
                });
                break;
            case NotFoundException _:
                code = HttpStatusCode.NotFound;
                break;
            case UnauthorizedAccessException _:
                code = HttpStatusCode.Unauthorized;
                break;
            default:
                result = JsonSerializer.Serialize(new
                {
                    succeeded = false,
                    message = "An error occurred processing your request"
                });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (string.IsNullOrEmpty(result))
        {
            result = JsonSerializer.Serialize(new
            {
                succeeded = false,
                message = exception.Message
            });
        }

        return context.Response.WriteAsync(result);
    }
}
```

## 📦 Required NuGet Packages

```xml
<!-- Add to MoviesAPI.csproj -->
<ItemGroup>
  <!-- CQRS & Mediator -->
  <PackageReference Include="MediatR" Version="12.2.0" />
  <PackageReference Include="MediatR.Extensions.Microsoft.DependencyInjection" Version="11.1.0" />
  
  <!-- Validation -->
  <PackageReference Include="FluentValidation" Version="11.9.0" />
  <PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
  
  <!-- Mapping -->
  <PackageReference Include="AutoMapper" Version="12.0.1" />
  <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
  
  <!-- Caching -->
  <PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="8.0.0" />
  
  <!-- Logging -->
  <PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
  <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
  
  <!-- API Versioning -->
  <PackageReference Include="Asp.Versioning.Mvc" Version="8.0.0" />
  <PackageReference Include="Asp.Versioning.Mvc.ApiExplorer" Version="8.0.0" />
</ItemGroup>
```

## 🔄 Migration Strategy

### Step 1: Add New Packages
```bash
cd Backend/MoviesAPI
dotnet add package MediatR
dotnet add package FluentValidation.AspNetCore
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

### Step 2: Create Folder Structure
Create all new folders as outlined above

### Step 3: Implement Phase 1 (DTOs, Validation, Mapping)
- Create DTOs for all entities
- Add validators
- Configure AutoMapper

### Step 4: Implement Phase 2 (CQRS)
- Create commands and queries
- Add handlers
- Update controllers

### Step 5: Testing
- Test each endpoint
- Verify validation works
- Check error handling

## 📊 Benefits

### Performance
- ✅ Caching reduces database calls
- ✅ CQRS optimizes read/write operations
- ✅ Async/await throughout

### Maintainability
- ✅ Clear separation of concerns
- ✅ Easy to test
- ✅ Consistent patterns

### Security
- ✅ Input validation
- ✅ Error handling doesn't expose internals
- ✅ Rate limiting prevents abuse

### Scalability
- ✅ CQRS allows separate scaling
- ✅ Caching reduces load
- ✅ Clean architecture supports growth

## 🎯 Next Steps

1. Review this plan
2. Approve architecture changes
3. Start with Phase 1 implementation
4. Test thoroughly
5. Deploy incrementally

---
**Status**: Ready for implementation
**Priority**: High
**Estimated Time**: 2-3 weeks for full implementation
