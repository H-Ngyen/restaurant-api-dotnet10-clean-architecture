using Application.Users;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("{UserEmail} [{UserId}] creating a new restaurant: {@Restaurant}", 
            currentUser!.Email,
            currentUser.Id,
            request);

        var restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId = currentUser.Id;
        
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}