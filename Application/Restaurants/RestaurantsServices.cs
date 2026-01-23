using Application.Restaurants.Dtos;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Restaurants;

internal class RestaurantsServices(IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsServices> logger) : IRestaurantsServices
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        var restaurantDtos = restaurants.Select(RestaurantDto.FromEntity);
        return restaurantDtos!;
    }
    public async Task<RestaurantDto?> GetRestaurantAsync(int id)
    {
        logger.LogInformation($"Getting restaurant {id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        var restaurantDto = RestaurantDto.FromEntity(restaurant);
        return restaurantDto;
    }
}
