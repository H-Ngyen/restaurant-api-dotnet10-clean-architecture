using MediatR;

namespace Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommand : IRequest
{
    public int Id { get; set; }
    public required Stream Stream { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
}