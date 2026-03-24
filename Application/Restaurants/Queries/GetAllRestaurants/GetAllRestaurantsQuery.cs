using Application.Common;
using Application.Restaurants.Dtos;
using Domain.Constraints;
using MediatR;

namespace Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
{
    public string? SearchPhrase { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
}