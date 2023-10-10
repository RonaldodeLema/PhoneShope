using Internals.Models.Enum;

namespace Internals.Models;


public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Status Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; }
    public virtual List<OrderItem>? OrderItems { get; set; }
}