using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);
        return restaurant;
    }

    public async Task<int> CreateAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);
        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task DeleteAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(int id)
        => await dbContext.Restaurants.AnyAsync(r => r.Id == id);
    
    public async Task SaveChanges() => await dbContext.SaveChangesAsync();
}
