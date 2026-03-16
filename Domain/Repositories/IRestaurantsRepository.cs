using Domain.Entities;
namespace Domain.Repositories;
public interface IRestaurantsRepository
{
    public Task<IEnumerable<Restaurant>> GetAllAsync();
    public Task<Restaurant?> GetByIdAsync(int id);
    public Task<int> CreateAsync(Restaurant restaurant);
    public Task DeleteAsync(Restaurant restaurant);
    public Task<bool> ExistAsync(int id);
    public Task SaveChanges();
}