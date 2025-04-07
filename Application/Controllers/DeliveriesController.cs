using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Context;

namespace Application.Controllers
{
    [ApiController]
    [Route("/deliveries")]
    public class DeliveriesController(DelivereaseDbContext context) : ControllerBase
    {
        // GET: api/Deliveries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDeliveries()
        {
            return await context.Deliveries.ToListAsync();
        }

        // GET: api/Deliveries/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Delivery>> GetDelivery(Guid id)
        {
            var delivery = await context.Deliveries.FindAsync(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return delivery;
        }

        // PUT: api/Deliveries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateDelivery(Guid id, Delivery delivery)
        {
            if (id != delivery.Id)
            {
                return BadRequest();
            }

            context.Entry(delivery).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Deliveries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Delivery>> AddDelivery(Delivery delivery)
        {
            context.Deliveries.Add(delivery);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetDelivery", new { id = delivery.Id }, delivery);
        }

        // DELETE: api/Deliveries/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDelivery(Guid id)
        {
            var delivery = await context.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }

            context.Deliveries.Remove(delivery);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryExists(Guid id)
        {
            return context.Deliveries.Any(e => e.Id == id);
        }
    }
}
