using Application.Restaurants;
using Application.Restaurants.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IRestaurantsServices restaurantsServices) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantsServices.GetAllRestaurantsAsync();
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRestaurant(int id) 
    {
        var restaurant = await restaurantsServices.GetRestaurantAsync(id);
        if(restaurant == null) 
            return NotFound();
        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantDto createRestaurantDto)
    {
        var id = await restaurantsServices.CreateRestaurantAsync(createRestaurantDto);
        return CreatedAtAction(nameof(GetRestaurant), new { id }, null);
    } 
}