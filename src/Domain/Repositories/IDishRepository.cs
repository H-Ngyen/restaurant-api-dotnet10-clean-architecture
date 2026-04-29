using Domain.Entities;

namespace Domain.Repositories;

public interface IDishRepository 
{
    public Task<int> CreateAsync(Dish entity);
    public Task DeleteAsync(Dish entity);
    public Task DeleteManyAsync(IEnumerable<Dish> entities);
    public Task SaveChanges();
}