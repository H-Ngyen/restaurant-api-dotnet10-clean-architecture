using Application.Users;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirementHandler(IUserContext userContext,
    ILogger<CreatedMultipleRestaurantsRequirementHandler> logger,
    IRestaurantsRepository restaurantsRepository) : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning("Authorization failed: User context is null.");
            return;
        }
        logger.LogInformation("User {UserEmail}, date of birth {DoB} - Handling CreatedMultipleRestaurantsRequirement",
            currentUser.Email,
            currentUser.DateOfBirth);

        var restaurant = await restaurantsRepository.GetAllAsync();

        var userRestaurantCreated = restaurant.Count(r => r.OwnerId == currentUser.Id);
        if (userRestaurantCreated >= requirement.MinimumRestaurantsCreated)
            context.Succeed(requirement);
        else
            context.Fail();

    }
}