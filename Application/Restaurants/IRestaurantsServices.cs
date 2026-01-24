using Application.Restaurants.Dtos;
using Domain.Entities;
namespace Application.Restaurants;
public interface IRestaurantsServices
{
    public Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();
    public Task<RestaurantDto?> GetRestaurantAsync(int id);
    public Task<int> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto);

}
