using FluentValidation;
using MoviesAPI.Application.DTOs.Requests.Tickets;

namespace MoviesAPI.Application.Validators.Tickets
{
    /// <summary>
    /// Validator for CreateTicketRequest
    /// </summary>
    public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
    {
        public CreateTicketRequestValidator()
        {
            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("Movie ID must be greater than 0");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0");

            RuleFor(x => x.WatchDateTime)
                .NotEmpty().WithMessage("Watch date/time is required")
                .GreaterThan(DateTime.Now).WithMessage("Watch date/time must be in the future");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .LessThanOrEqualTo(10000).WithMessage("Price cannot exceed 10,000");

            RuleFor(x => x.HallSeatId)
                .GreaterThan(0).WithMessage("Hall seat ID must be greater than 0");
        }
    }

    /// <summary>
    /// Validator for BookSeatsRequest
    /// </summary>
    public class BookSeatsRequestValidator : AbstractValidator<BookSeatsRequest>
    {
        public BookSeatsRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters");

            RuleFor(x => x.ScreeningId)
                .GreaterThan(0).WithMessage("Screening ID must be greater than 0");

            RuleFor(x => x.SelectedSeatIds)
                .NotEmpty().WithMessage("At least one seat must be selected")
                .Must(seats => seats != null && seats.Count > 0)
                .WithMessage("At least one seat must be selected")
                .Must(seats => seats != null && seats.Count <= 10)
                .WithMessage("Cannot book more than 10 seats at once")
                .Must(seats => seats != null && seats.Distinct().Count() == seats.Count)
                .WithMessage("Duplicate seat IDs are not allowed");
        }
    }

    /// <summary>
    /// Validator for CancelTicketRequest
    /// </summary>
    public class CancelTicketRequestValidator : AbstractValidator<CancelTicketRequest>
    {
        public CancelTicketRequestValidator()
        {
            RuleFor(x => x.TicketId)
                .GreaterThan(0).WithMessage("Ticket ID must be greater than 0");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0");

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters");
        }
    }
}
