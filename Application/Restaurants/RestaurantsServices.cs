using System.Formats.Asn1;
using Application.Restaurants.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Restaurants;

internal class RestaurantsServices(IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsServices> logger,
    IMapper mapper) : IRestaurantsServices
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        return restaurantDtos;
    }
    public async Task<RestaurantDto?> GetRestaurantAsync(int id)
    {
        logger.LogInformation($"Getting restaurant {id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);
        return restaurantDto;
    }

    public async Task<int> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto)
    {
        logger.LogInformation($"Creating a new restaurant");
        var restaurant = mapper.Map<Restaurant>(createRestaurantDto);
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}
