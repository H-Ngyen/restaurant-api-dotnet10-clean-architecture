using MediatR;

namespace Application.Dishes.Commands.DeleteAllDish;

public class DeleteAllDishForRestaurantCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
}