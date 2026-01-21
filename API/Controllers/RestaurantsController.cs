using Application.Restaurants;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IRestaurantsServices restaurantsServices) : ControllerBase
{
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantsServices.GetAllRestaurantsAsync();
        return Ok(restaurants);
    }
}