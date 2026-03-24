using MediatR;

namespace Application.Dishes.Commands.CreateDish;
 
public class CreateDishCommand : IRequest<int>
{
    public int RestaurantId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
}