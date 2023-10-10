namespace Internals.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PhoneDetailsId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Order Order { get; set; }
    public PhoneDetails PhoneDetails { get; set; }
}