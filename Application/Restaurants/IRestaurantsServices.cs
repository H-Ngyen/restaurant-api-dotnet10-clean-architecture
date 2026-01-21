using Domain.Entities;
namespace Application.Restaurants;
public interface IRestaurantsServices
{
    public Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
}
