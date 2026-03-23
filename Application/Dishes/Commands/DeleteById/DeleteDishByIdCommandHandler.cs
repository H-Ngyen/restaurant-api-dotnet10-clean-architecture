using Domain.Constraints;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Dishes.Commands.DeleteById;

public class DeleteDishByIdCommandHandler(ILogger<DeleteDishByIdCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishRepository dishRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishByIdCommand>
{
    public async Task Handle(DeleteDishByIdCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing dishes: {DishId} from restaurant: {RestaurantId}", 
            request.DishId,
            request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if(!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();

        var dish = restaurant.Dishes.Find(d => d.Id == request.DishId)
            ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        await dishRepository.DeleteAsync(dish);
    }
}