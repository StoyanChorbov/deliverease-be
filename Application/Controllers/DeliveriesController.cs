using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO.Delivery;
using Service;

namespace Application.Controllers;

[ApiController]
[Route("/deliveries")]
public class DeliveriesController(DeliveryService deliveryService) : ControllerBase
{
    // Add a new delivery to the database
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddDelivery([FromBody] DeliveryAddDto deliveryAddDto)
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized();
        }

        await deliveryService.AddDeliveryAsync(
            deliveryAddDto,
            User.Identity?.Name ?? throw new Exception("User not found")
        );
        return Ok();
    }

    // Get delivery by id
    [HttpGet("{id:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<DeliveryDto>> GetDelivery(Guid id)
    {
        return await deliveryService.GetDeliveryAsync(id);
    }

    // Get deliveries by starting and ending location
    [HttpGet("/deliveries/locations/{startingLocationRegion}/{endingLocationRegion}")]
    public async Task<ActionResult<List<DeliveryDto>>> GetDeliveriesByLocations(
        string startingLocationRegion, string endingLocationRegion)
    {
        var deliveries =
            await deliveryService.GetAllByStartingAndEndingLocation(startingLocationRegion, endingLocationRegion);
        return Ok(deliveries);
    }

    // Update the delivery using id and dto
    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateDelivery([FromRoute] Guid id, [FromBody] DeliveryDto deliveryDto)
    {
        await deliveryService.UpdateDeliveryAsync(id, deliveryDto);
        return Ok();
    }

    // Delete the delivery
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteDelivery(Guid id)
    {
        await deliveryService.DeleteDeliveryAsync(id);
        return NoContent();
    }
}