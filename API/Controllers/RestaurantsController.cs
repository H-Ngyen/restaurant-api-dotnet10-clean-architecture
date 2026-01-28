using Application.Restaurants;
using Application.Restaurants.Commands.CreateRestaurant;
using Application.Restaurants.Commands.DeleteRestaurant;
using Application.Restaurants.Commands.UpdateRestaurant;
using Application.Restaurants.Dtos;
using Application.Restaurants.Queries.GetAllRestaurants;
using Application.Restaurants.Queries.GetRestaurantById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRestaurant(int id) 
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        if(restaurant == null) 
            return NotFound();
        return Ok(restaurant);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand createRestaurantCommand)
    {
        var id = await mediator.Send(createRestaurantCommand);
        return CreatedAtAction(nameof(GetRestaurant), new { id }, null);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRestaurant(int id) 
    {
        var isDelete = await mediator.Send(new DeleteRestaurantCommand(id));
        if(!isDelete) 
            return NotFound();
        return NoContent();
    } 

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateRestaurant(int id, UpdateRestaurantCommand command)
    {
        command.Id = id;
        var isUpdate = await mediator.Send(command);
        if(!isUpdate) 
            return NotFound();
        return NoContent();
    }
}