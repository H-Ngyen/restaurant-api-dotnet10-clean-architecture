using Application.Restaurants.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(ILogger<GetRestaurantByIdQueryHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper,
    IObjectStorageService objectStorageService) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant {RestaurantId}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

        restaurantDto.LogoSasUrl = await objectStorageService.GetDownloadUrlAsync(restaurant.LogoUrl);

        return restaurantDto;
    }
}