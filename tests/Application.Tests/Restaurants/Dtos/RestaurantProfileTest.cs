using Application.Restaurants.Commands.CreateRestaurant;
using Application.Restaurants.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
namespace Application.Tests.Restaurants.Dtos.Tests;

public class RestaurantProfileTest
{
    // private readonly TestSetup _testSetup;
    private IMapper _mapper;
    public RestaurantProfileTest()
    {
        // _testSetup = testSetup;
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole();
                // .AddFilter("LuckyPennySoftware.AutoMapper.License", LogLevel.None);
        });

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaunrantsProfile>();
        }, loggerFactory);

        _mapper = configuration.CreateMapper();
    }
    
    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // arrange
        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test restaurant name",
            Description = "Test Description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "123456789",
            Address = new Address
            {
                City = "Test city",
                Street = "Test street",
                PostalCode = "12345"
            }
        };

        // act

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // assert

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test restaurant name",
            Description = "Test Description",
            Category = "Test category",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "123456789",
            City = "Test city",
            Street = "Test street",
            PostalCode = "12345"
        };

        // act

        var restaurant = _mapper.Map<Restaurant>(command);

        // assert

        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address?.City.Should().Be(command.City);
        restaurant.Address?.Street.Should().Be(command.Street);
        restaurant.Address?.PostalCode.Should().Be(command.PostalCode);
    }
}