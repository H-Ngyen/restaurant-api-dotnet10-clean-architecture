using Application.Restaurants.Dtos;
using FluentValidation;

namespace Application.Restaurants.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly List<string> validCategory = ["Italia", "Mexico", "Japanese", "American", "Indian"];
    public CreateRestaurantDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(dto => dto.Category)
            .Must(validCategory.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories.");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d\d{3}")
            .WithMessage("Please provide a valid postal code (XX-XXX).");
    }
}