using Application.Restaurants;
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
}