using System.ComponentModel.Design;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Restaurants;

internal class RestaurantsServices(IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsServices> logger) : IRestaurantsServices
{
    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        return restaurants;
    }
}
