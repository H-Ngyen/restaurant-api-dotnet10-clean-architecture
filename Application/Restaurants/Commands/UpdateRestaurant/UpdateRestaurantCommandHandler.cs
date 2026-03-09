using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating the restaurant with id: {RestaurantId} with {@UpdateRestaurant}", request.Id, request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if(restaurant == null) 
            return false;
        mapper.Map(request, restaurant);
        await restaurantsRepository.SaveChanges();
        return true;
    }
}