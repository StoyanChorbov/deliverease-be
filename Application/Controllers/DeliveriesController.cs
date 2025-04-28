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
    // POST: api/Deliveries
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddDelivery([FromBody] DeliveryAddDto deliveryAddDto)
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized();
        }

        await deliveryService.AddDeliveryAsync(deliveryAddDto, User.Identity?.Name ?? throw new Exception("User not found"));
        return Ok();
    }
    
    // GET: api/Deliveries
    [HttpGet]
    public async Task<ActionResult<List<FindableDeliveryDto>>> GetDeliveries()
    {
        var deliveries = await deliveryService.GetAllDeliveriesAsync();
        return Ok(deliveries);
    }

    // GET: api/Deliveries/5
    [HttpGet("{id:guid}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<DeliveryDto>> GetDelivery(Guid id)
    {
        return await deliveryService.GetDeliveryAsync(id);
    }
    
    [HttpGet("/deliveries/locations/{startingLocationRegion:int}/{endingLocationRegion:int}")]
    public async Task<ActionResult<List<DeliveryDto>>> GetDeliveriesByLocations(
        int startingLocationRegion, int endingLocationRegion)
    {
        var deliveries = await deliveryService.GetAllByStartingAndEndingLocation(startingLocationRegion, endingLocationRegion);
        return Ok(deliveries);
    }

    // PUT: api/Deliveries/5
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateDelivery([FromRoute] Guid id, [FromBody] DeliveryDto deliveryDto)
    {
        await deliveryService.UpdateDeliveryAsync(id, deliveryDto);
        return Ok();
    }

    // DELETE: api/Deliveries/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDelivery(Guid id)
    {
        await deliveryService.DeleteDeliveryAsync(id);
        return NoContent();
    }
}