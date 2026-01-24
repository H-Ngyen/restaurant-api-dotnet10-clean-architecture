using Domain.Entities;
namespace Domain.Repositories;
public interface IRestaurantsRepository
{
    public Task<IEnumerable<Restaurant>> GetAllAsync();
    public Task<Restaurant?> GetByIdAsync(int id);
    public Task<int> CreateAsync(Restaurant restaurant);
}