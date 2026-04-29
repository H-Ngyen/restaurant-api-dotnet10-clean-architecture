using Domain.Constraints;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler(ILogger<UploadRestaurantLogoCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IObjectStorageService storageService) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Uploading restaurant logo for id: {RestaurantId}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Restaurant), $"{request.Id}");
        
        if(!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();
        
        var logoUrl = await storageService.UploadToStorageAsync(request.Stream, request.FileName, request.ContentType);
        
        restaurant.LogoUrl = logoUrl;
        
        await restaurantsRepository.SaveChanges();
    }
}