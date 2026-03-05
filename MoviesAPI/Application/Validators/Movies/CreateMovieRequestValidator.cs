using FluentValidation;
using MoviesAPI.Application.DTOs.Requests.Movies;

namespace MoviesAPI.Application.Validators.Movies
{
    /// <summary>
    /// Validator for CreateMovieRequest
    /// </summary>
    public class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
    {
        public CreateMovieRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Movie name is required")
                .MaximumLength(200).WithMessage("Movie name cannot exceed 200 characters")
                .MinimumLength(1).WithMessage("Movie name must be at least 1 character");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes")
                .LessThanOrEqualTo(600).WithMessage("Duration cannot exceed 600 minutes (10 hours)");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Release date is required")
                .LessThanOrEqualTo(DateTime.Now.AddYears(5))
                .WithMessage("Release date cannot be more than 5 years in the future");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount cannot be negative")
                .LessThanOrEqualTo(10000).WithMessage("Amount cannot exceed 10,000");

            RuleFor(x => x.PosterPath)
                .NotEmpty().WithMessage("Poster path is required")
                .MaximumLength(500).WithMessage("Poster path cannot exceed 500 characters");

            RuleFor(x => x.Plot)
                .NotEmpty().WithMessage("Plot is required")
                .MinimumLength(10).WithMessage("Plot must be at least 10 characters")
                .MaximumLength(2000).WithMessage("Plot cannot exceed 2000 characters");

            RuleFor(x => x.Actors)
                .NotEmpty().WithMessage("At least one actor is required")
                .MaximumLength(1000).WithMessage("Actors list cannot exceed 1000 characters");

            RuleFor(x => x.Directors)
                .NotEmpty().WithMessage("At least one director is required")
                .MaximumLength(500).WithMessage("Directors list cannot exceed 500 characters");

            RuleFor(x => x.Genres)
                .NotEmpty().WithMessage("At least one genre is required")
                .Must(genres => genres != null && genres.Count > 0)
                .WithMessage("At least one genre must be specified")
                .Must(genres => genres != null && genres.Count <= 5)
                .WithMessage("Cannot have more than 5 genres");
        }
    }

    /// <summary>
    /// Validator for UpdateMovieRequest
    /// </summary>
    public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
    {
        public UpdateMovieRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Movie name is required")
                .MaximumLength(200).WithMessage("Movie name cannot exceed 200 characters");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes")
                .LessThanOrEqualTo(600).WithMessage("Duration cannot exceed 600 minutes");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount cannot be negative");

            RuleFor(x => x.Plot)
                .NotEmpty().WithMessage("Plot is required")
                .MinimumLength(10).WithMessage("Plot must be at least 10 characters");

            RuleFor(x => x.Genres)
                .NotEmpty().WithMessage("At least one genre is required");
        }
    }

    /// <summary>
    /// Validator for RateMovieRequest
    /// </summary>
    public class RateMovieRequestValidator : AbstractValidator<RateMovieRequest>
    {
        public RateMovieRequestValidator()
        {
            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("Movie ID must be greater than 0");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 10).WithMessage("Rating must be between 1 and 10");
        }
    }
}
