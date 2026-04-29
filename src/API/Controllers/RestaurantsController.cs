using Application.Restaurants.Commands.CreateRestaurant;
using Application.Restaurants.Commands.DeleteRestaurant;
using Application.Restaurants.Commands.UpdateRestaurant;
using Application.Restaurants.Commands.UploadRestaurantLogo;
using Application.Restaurants.Dtos;
using Application.Restaurants.Queries.GetAllRestaurants;
using Application.Restaurants.Queries.GetRestaurantById;
using Domain.Constraints;
using Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    // [Authorize(Policy = PolicyNames.CreatedAtleast2Restaurants)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        var restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = PolicyNames.HasNationality)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurant([FromRoute] int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> CreateRestaurant(CreateRestaurantCommand createRestaurantCommand)
    {
        var id = await mediator.Send(createRestaurantCommand);
        return CreatedAtAction(nameof(GetRestaurant), new { id }, null);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRestaurant(int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateRestaurant(int id, UpdateRestaurantCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:int}/logo")]
    // [AllowAnonymous]
    public async Task<ActionResult> UploadLogo(int id, [FromForm] IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var command = new UploadRestaurantLogoCommand()
        {
            Id = id,
            FileName = $"{id}-{file.FileName}",
            Stream = stream,
            ContentType = file.ContentType
        };
        await mediator.Send(command);
        return NoContent();
    }

}