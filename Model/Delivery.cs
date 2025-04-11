using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class Delivery : BaseEntity
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public DeliveryCategory Category { get; set; }
    
    // [ForeignKey("SenderId")]
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
    
    public List<User> Recipients { get; set; }
    
    // [ForeignKey("DelivererId")]
    public Guid DelivererId { get; set; }
    public User Deliverer { get; set; }
    
    public Guid StartingLocationId { get; set; }
    public Location StartingLocation { get; set; }
    
    public Guid EndingLocationId { get; set; }
    public Location EndingLocation { get; set; }
    
    public bool IsFragile { get; set; }
    
    public DateTime? DeliveryDate { get; set; }

    public Delivery()
    {
    }
}