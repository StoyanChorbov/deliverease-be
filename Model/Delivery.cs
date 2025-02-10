using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class Delivery : BaseEntity
{
    [Required]
    public string StartingPoint { get; set; }
    
    [Required]
    public string Destination { get; set; }
    
    [Required]
    [ForeignKey("SenderId")]
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
    
    [Required]
    [ForeignKey("DelivererId")]
    public Guid DelivererId { get; set; }
    public User Deliverer { get; set; }

    public Delivery()
    {
    }
    
    public Delivery(string startingPoint, string destination, Guid senderId, Guid delivererId)
    {
        StartingPoint = startingPoint;
        Destination = destination;
        SenderId = senderId;
        DelivererId = delivererId;
    }
}