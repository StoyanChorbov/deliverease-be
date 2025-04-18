using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO.Delivery;
using Service;

namespace Application.Controllers;

[ApiController]
[Route("/deliveries")]
[Authorize]
public class DeliveriesController(DeliveryService deliveryService) : ControllerBase
{
    // GET: api/Deliveries
    [HttpGet]
    public async Task<ActionResult<List<Delivery>>> GetDeliveries()
    {
        var deliveries = await deliveryService.GetAllDeliveriesAsync();
        return Ok(deliveries);
    }

    // GET: api/Deliveries/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeliveryDto>> GetDelivery(Guid id)
    {
        return await deliveryService.GetDeliveryAsync(id);
    }

    // PUT: api/Deliveries/5
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateDelivery(Guid id, DeliveryDto deliveryDto)
    {
        await deliveryService.UpdateDeliveryAsync(id, deliveryDto);
        return Ok();
    }

    // POST: api/Deliveries
    [HttpPost]
    public async Task<IActionResult> AddDelivery([FromBody] DeliveryAddDto deliveryAddDto,
        [FromHeader] string authorization)
    {
        Console.WriteLine(authorization);

        var hasTokenExpired = UserService.CheckTokenValidity(authorization);
        if (hasTokenExpired != null)
            return Unauthorized(hasTokenExpired);

        await deliveryService.AddDeliveryAsync(deliveryAddDto, authorization);
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