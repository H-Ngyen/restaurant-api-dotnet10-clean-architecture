using Domain.Entities;
using FluentValidation;

namespace Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private readonly int[] allowPageSizes = [5, 10, 15, 20, 30];
    private readonly string[] allowSortByColumnNames = [
        nameof(Restaurant.Name),
        nameof(Restaurant.Category),
        nameof(Restaurant.Description)
    ];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(dto => dto.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(dto => dto.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size should be [{string.Join(", ", allowPageSizes)}]");

        RuleFor(dto => dto.SortBy)
            .Must(value => allowSortByColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optional, or must be in [{string.Join(", ", allowSortByColumnNames)}]");
    }
}