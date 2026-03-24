using MediatR;

namespace Application.Dishes.Commands.DeleteById;

public class DeleteDishByIdCommand(int restaurantId, int dishId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
    public int DishId { get; } = dishId;
}