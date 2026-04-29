using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class DishRepository(RestaurantsDbContext dbContext) : IDishRepository
{
    public async Task<int> CreateAsync(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await SaveChanges();
        return entity.Id;
    }

    public async Task DeleteAsync(Dish entity)
    => await dbContext.Dishes
        .Where(e => e.Id == entity.Id && e.RestaurantId == entity.RestaurantId)
        .ExecuteDeleteAsync();

    public async Task DeleteManyAsync(IEnumerable<Dish> entities)
    {
        dbContext.Dishes.RemoveRange(entities);
        await SaveChanges();
    }

    public async Task SaveChanges() => await dbContext.SaveChangesAsync();
}