using Application.Dishes.Dtos;
using MediatR;

namespace Application.Dishes.Queries.GetAllDishForRestaurant;

public class GetAllDishForRestaurantQuery(int restaurantId) : IRequest<IEnumerable<DishDto>>
{
    public int RestaurantId { get; } = restaurantId;
}