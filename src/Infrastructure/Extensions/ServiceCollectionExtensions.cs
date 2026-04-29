using Domain.Constraints;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using Infrastructure.Authorization;
using Infrastructure.Authorization.Requirements;
using Infrastructure.Authorization.Services;
using Infrastructure.Configurations;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Seeders;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var ConnectionString = config.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options => options.UseNpgsql(ConnectionString));

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IObjectStorageService, ObjectStorageService>();

        // setting minio
        var minioSettings = config.GetSection("MinIO").Get<ObjectStorageSettings>()
            ?? throw new Exception("MinIO settings are missing in appsettings.json");
        // register MinioClient is Singleton (Injectable)
        services.AddSingleton(sp =>
        {
            return new MinioClient()
                .WithEndpoint(minioSettings.Endpoint)
                .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
                // .WithSSL() 
                .Build();
        });

        // configure settings
        services.Configure<ObjectStorageSettings>(config.GetSection("MinIO"));

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
