using FluentValidation;
using Hotel.Application.DTOs;

namespace Hotel.Application.Validators;

public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.GuestId)
            .GreaterThan(0)
            .WithMessage("Guest ID is required");

        RuleFor(x => x.RoomId)
            .GreaterThan(0)
            .WithMessage("Room ID is required");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be later than start date"); // ✅ Zgodnie z wytycznymi 8.2
    }
}