using Application.Users;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;

namespace Infrastructure.Tests.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirementHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // arrange
        var loggerMock = new Mock<ILogger<CreatedMultipleRestaurantsRequirementHandler>>();

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurant = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "3"
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurant);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(userContextMock.Object, loggerMock.Object, restaurantsRepositoryMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null!, null);

        // act

        await handler.HandleAsync(context);

        // assert

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldFail()
    {
        // arrange
        var loggerMock = new Mock<ILogger<CreatedMultipleRestaurantsRequirementHandler>>();

        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurant = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "3"
            },
            new()
            {
                OwnerId = "3"
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurant);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(userContextMock.Object, loggerMock.Object, restaurantsRepositoryMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null!, null);

        // act

        await handler.HandleAsync(context);

        // assert

        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }
}