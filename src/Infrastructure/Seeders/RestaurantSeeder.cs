using Domain.Constraints;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Users.Any())
            {
                var adminEmail = "admin@test.com";
                var ownerEmail = "owner@test.com";
                var userEmail = "user@test.com";
                var password = "123";

                var users = GetUsers(adminEmail, ownerEmail, userEmail, password);
                dbContext.Users.AddRange(users);
                await dbContext.SaveChangesAsync();

                // Assign roles to users
                var adminUser = dbContext.Users.First(u => u.Email == adminEmail);
                var ownerUser = dbContext.Users.First(u => u.Email == ownerEmail);
                var normalUser = dbContext.Users.First(u => u.Email == userEmail);

                var adminRole = dbContext.Roles.First(r => r.Name == UserRoles.Admin);
                var ownerRole = dbContext.Roles.First(r => r.Name == UserRoles.Owner);
                var userRole = dbContext.Roles.First(r => r.Name == UserRoles.User);

                dbContext.UserRoles.AddRange(
                    new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = adminRole.Id },
                    new IdentityUserRole<string> { UserId = ownerUser.Id, RoleId = ownerRole.Id },
                    new IdentityUserRole<string> { UserId = normalUser.Id, RoleId = userRole.Id }
                );
            }

            if (!dbContext.Restaurants.Any())
            {
                var ownerUser = dbContext.Users.First(u => u.Email == "owner@test.com");
                var restaurants = GetRestaurants(ownerUser.Id);
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }
    private IEnumerable<User> GetUsers(string adminEmail, string ownerEmail, string userEmail, string password)
    {
        var hasher = new PasswordHasher<User>();

        var adminUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = adminEmail,
            NormalizedUserName = adminEmail.ToUpper(),
            Email = adminEmail,
            NormalizedEmail = adminEmail.ToUpper(),
            EmailConfirmed = true,
            Nationality = AppClaimTypes.VietNam,
            DateOfBirth = new DateOnly(1990, 1, 1),
            SecurityStamp = Guid.NewGuid().ToString()
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, password);

        var ownerUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = ownerEmail,
            NormalizedUserName = ownerEmail.ToUpper(),
            Email = ownerEmail,
            NormalizedEmail = ownerEmail.ToUpper(),
            EmailConfirmed = true,
            Nationality = AppClaimTypes.VietNam,
            DateOfBirth = new DateOnly(1985, 5, 15),
            SecurityStamp = Guid.NewGuid().ToString()
        };
        ownerUser.PasswordHash = hasher.HashPassword(ownerUser, password);

        var normalUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = userEmail,
            NormalizedUserName = userEmail.ToUpper(),
            Email = userEmail,
            NormalizedEmail = userEmail.ToUpper(),
            EmailConfirmed = true,
            Nationality = AppClaimTypes.VietNam,
            DateOfBirth = new DateOnly(2000, 12, 25),
            SecurityStamp = Guid.NewGuid().ToString()
        };
        normalUser.PasswordHash = hasher.HashPassword(normalUser, password);

        return new List<User> { adminUser, ownerUser, normalUser };
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = [
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper()
            },
            new(UserRoles.Owner)
            {
                NormalizedName = UserRoles.Owner.ToUpper()
            },
            new(UserRoles.User)
            {
                NormalizedName = UserRoles.User.ToUpper()
            }
        ];
        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants(string ownerId)
    {
        List<Restaurant> restaurants = [
            new()
            {
                OwnerId = ownerId,
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes =
                [
                    new ()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pcs.)",
                        Price = 10.30M,
                    },

                    new ()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pcs.)",
                        Price = 5.30M,
                    },
                ],
                Address = new ()
                {
                    City = "London",
                    Street = "Cork St 5",
                    PostalCode = "WC2N 5DU"
                },

            },
            new ()
            {
                OwnerId = ownerId,
                Name = "McDonald",
                Category = "Fast Food",
                Description =
                    "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "London",
                    Street = "Boots 193",
                    PostalCode = "W1F 8SR"
                }
            }
        ];
        return restaurants;
    }
}
