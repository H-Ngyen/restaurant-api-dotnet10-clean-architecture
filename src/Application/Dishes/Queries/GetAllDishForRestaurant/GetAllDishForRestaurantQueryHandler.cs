using Application.Dishes.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Dishes.Queries.GetAllDishForRestaurant;

public class GetAllDishForRestaurantQueryHandler(ILogger<GetAllDishForRestaurantQueryHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper) : IRequestHandler<GetAllDishForRestaurantQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetAllDishForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dishes for restaurant with id: {RestaurantId}", request.RestaurantId);
        var restaurants = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Dish), request.RestaurantId.ToString());
        
        var dishDtos = mapper.Map<IEnumerable<DishDto>>(restaurants.Dishes);

        return dishDtos;
    }
}