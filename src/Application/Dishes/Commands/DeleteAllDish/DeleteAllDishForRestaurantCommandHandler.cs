using Domain.Constraints;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Dishes.Commands.DeleteAllDish;

public class DeleteAllDishForRestaurantCommandHandler(ILogger<DeleteAllDishForRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishRepository dishRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteAllDishForRestaurantCommand>
{
    public async Task Handle(DeleteAllDishForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing all dishes from restaurant: {RestaurantId}", request.RestaurantId);
   
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        
        if(!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();

        await dishRepository.DeleteManyAsync(restaurant.Dishes);
    }
}