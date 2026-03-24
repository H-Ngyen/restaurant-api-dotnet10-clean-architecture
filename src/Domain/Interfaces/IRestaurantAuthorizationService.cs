using Domain.Constraints;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IRestaurantAuthorizationService
{
    bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation);    
}