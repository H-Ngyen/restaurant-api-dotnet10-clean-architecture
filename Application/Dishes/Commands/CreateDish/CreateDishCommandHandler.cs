using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishRepository dishRepository,
    IMapper mapper) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new dish {@DishRequest}", request);
        var restaurantExisting = await restaurantsRepository.ExistAsync(request.RestaurantId);
        if(!restaurantExisting)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        
        var newDish = mapper.Map<Dish>(request);
        var dishId = await dishRepository.CreateAsync(newDish);
        return dishId;
    }
}