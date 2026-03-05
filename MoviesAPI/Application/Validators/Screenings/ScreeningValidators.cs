using FluentValidation;
using MoviesAPI.Application.DTOs.Requests.Screenings;

namespace MoviesAPI.Application.Validators.Screenings
{
    /// <summary>
    /// Validator for CreateScreeningRequest
    /// </summary>
    public class CreateScreeningRequestValidator : AbstractValidator<CreateScreeningRequest>
    {
        public CreateScreeningRequestValidator()
        {
            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("Movie ID must be greater than 0");

            RuleFor(x => x.HallId)
                .GreaterThan(0).WithMessage("Hall ID must be greater than 0");

            RuleFor(x => x.ScreeningDateTime)
                .NotEmpty().WithMessage("Screening date/time is required")
                .GreaterThan(DateTime.Now).WithMessage("Screening date/time must be in the future")
                .LessThan(DateTime.Now.AddYears(1)).WithMessage("Screening date/time cannot be more than 1 year in the future");
        }
    }

    /// <summary>
    /// Validator for UpdateScreeningRequest
    /// </summary>
    public class UpdateScreeningRequestValidator : AbstractValidator<UpdateScreeningRequest>
    {
        public UpdateScreeningRequestValidator()
        {
            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("Movie ID must be greater than 0");

            RuleFor(x => x.HallId)
                .GreaterThan(0).WithMessage("Hall ID must be greater than 0");

            RuleFor(x => x.ScreeningDateTime)
                .NotEmpty().WithMessage("Screening date/time is required");

            RuleFor(x => x.TotalTickets)
                .GreaterThan(0).WithMessage("Total tickets must be greater than 0")
                .LessThanOrEqualTo(1000).WithMessage("Total tickets cannot exceed 1000");

            RuleFor(x => x.AvailableTickets)
                .GreaterThanOrEqualTo(0).WithMessage("Available tickets cannot be negative")
                .LessThanOrEqualTo(x => x.TotalTickets)
                .WithMessage("Available tickets cannot exceed total tickets");
        }
    }
}
