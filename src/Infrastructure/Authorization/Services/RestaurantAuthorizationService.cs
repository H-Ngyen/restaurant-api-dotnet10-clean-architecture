using Application.Users;
using Domain.Constraints;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        if (user == null) return false;

        logger.LogInformation("Authorizing user {UserEmail}, to operation {Operation} for restaurant {RestaurantName}",
            user.Email,
            resourceOperation,
            restaurant.Name);

        if (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.Read)
        {
            logger.LogInformation("Create/Read operation - successful authorization");
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;
        }

        if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update)
            && user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant owner - successful authorization");
            return true;
        }
        return false;
    }

    // public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    // {
    //     var user = userContext.GetCurrentUser();

    //     if (user == null) return false;

    //     var isAuthorized = resourceOperation switch
    //     {
    //         ResourceOperation.Read => true,

    //         ResourceOperation.Create => true, 

    //         ResourceOperation.Update or ResourceOperation.Delete =>
    //             user.IsInRole(UserRoles.Admin) || user.Id == restaurant.OwnerId,

    //         _ => false
    //     };

    //     logger.LogInformation(
    //         "Authorization result: {Result} | User: {Email} | Operation: {Operation} | Restaurant: {Restaurant}",
    //         isAuthorized,
    //         user.Email,
    //         resourceOperation,
    //         restaurant.Name
    //     );

    //     return isAuthorized;
    // }
}