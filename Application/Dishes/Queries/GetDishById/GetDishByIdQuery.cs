using Application.Dishes.Dtos;
using MediatR;

namespace Application.Dishes.Queries.GetDishById;

public class GetDishByIdQuery(int restaurantId, int dishId) : IRequest<DishDto>
{
    public int RestaurantId { get; } = restaurantId;
    public int DishId { get; } = dishId;
    
}