using Application.Restaurants.Commands.UpdateRestaurant;
using AutoMapper;
using Domain.Constraints;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Restaurants.commands.UpdateRestaurant;

public class UpdateRestaurantCommandTest
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandTest()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _restaurantRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "New name",
            Description = "New description",
            HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Old name",
            Description = "Old description",
        };

        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurant.Id))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(r => r.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);

        // act
        
        await _handler.Handle(command, CancellationToken.None);

        // assert

        _restaurantRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoExistingRestaurant_ShouldThrowNotFoundException()
    {
        // arrange 
        var restaurantId = 1;
        var request = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null);

        // act 
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // assert 
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"{nameof(Restaurant)} with id: {request.Id} doesn't exist");
    }
    
    [Fact]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        // arrange
        var restaurantId = 3;
        var request = new UpdateRestaurantCommand()
        {
            Id = restaurantId  
        };

        var existingRestaurant = new Restaurant()
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync(existingRestaurant);

        _restaurantAuthorizationServiceMock
            .Setup(a => a.Authorize(existingRestaurant, ResourceOperation.Update))
                .Returns(false);

        // act

        Func<Task> act = async() => await _handler.Handle(request, CancellationToken.None);

        // assert

        await act.Should().ThrowAsync<ForbidException>();
    }
}