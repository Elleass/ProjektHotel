using FluentValidation;
using Hotel.Application.DTOs;

namespace Hotel.Application.Validators;

public class CreateRoomDtoValidator : AbstractValidator<CreateRoomDto>
{
    public CreateRoomDtoValidator()
    {
        RuleFor(x => x.RoomNumber)
            .GreaterThan(0)
            .WithMessage("Room number must be greater than 0");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Room type is required")
            .MaximumLength(50)
            .WithMessage("Room type cannot exceed 50 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0")
            .LessThanOrEqualTo(10000)
            .WithMessage("Price cannot exceed 10,000");
    }
}