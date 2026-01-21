using Domain.Entities;

namespace Domain.Repositories;

public interface IRestaurantsRepository
{
    public Task<IEnumerable<Restaurant>> GetAllAsync();
}