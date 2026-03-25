using Domain.Constraints;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using Infrastructure.Authorization;
using Infrastructure.Authorization.Requirements;
using Infrastructure.Authorization.Services;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var ConnectionString = config.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options => options.UseSqlServer(ConnectionString));

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishRepository, DishRepository>();

        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality, 
                builder => builder.RequireClaim(
                    AppClaimTypes.Nationality, 
                    AppClaimTypes.VietNam, 
                    AppClaimTypes.German, 
                    AppClaimTypes.Polish
                ))
            .AddPolicy(PolicyNames.AtLeast20, 
                builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
            .AddPolicy(PolicyNames.CreatedAtleast2Restaurants,
                builder => builder.AddRequirements(new CreatedMultipleRestaurantsRequirement(2)));
        
        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
    }
}
