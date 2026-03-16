using Application.Dishes.Commands.CreateDish;
using Application.Dishes.Commands.DeleteAllDish;
using Application.Dishes.Commands.DeleteById;
using Application.Dishes.Dtos;
using Application.Dishes.Queries.GetAllDishForRestaurant;
using Application.Dishes.Queries.GetDishById;
using Application.Restaurants.Queries.GetRestaurantById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        var dishId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetDishById), new { restaurantId, dishId }, dishId);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllDishForRestaurant([FromRoute] int restaurantId)
    {
        var dishDtos = await mediator.Send(new GetAllDishForRestaurantQuery(restaurantId));
        return Ok(dishDtos);
    }

    [HttpGet("{dishId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishDto>> GetDishById([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = await mediator.Send(new GetDishByIdQuery(restaurantId, dishId));
        return Ok(dish);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteAllDishForRestaurant([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteAllDishForRestaurantCommand(restaurantId));
        return NoContent();
    }

    [HttpDelete("{dishId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteDishById([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        await mediator.Send(new DeleteDishByIdCommand(restaurantId, dishId));
        return NoContent();
    }
}