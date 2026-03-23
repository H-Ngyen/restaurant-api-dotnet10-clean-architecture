using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirement(int minimumRestaurantsCreated) : IAuthorizationRequirement
{
    public int MinimumRestaurantsCreated { get; } = minimumRestaurantsCreated;
}