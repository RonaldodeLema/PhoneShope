using System.ComponentModel;
using Internals.Models.Enum;

namespace Internals.Models;


public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Status Status { get; set; }
    
    public Method Method { get; set; }
    public int PromotionId { get; set; }
    public string? Note { get; set; }
    [DefaultValue("COD")]
    public string? PayCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; }    
    public Promotion Promotion { get; set; }

    public virtual List<OrderItem>? OrderItems { get; set; }
    
    public void SetDateTime()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    public void SetUpdateDate()
    {
        UpdatedAt = DateTime.Now;
    }

    public double SubTotal()
    {
        return OrderItems?.Sum(s => s.Price * s.Quantity) ?? 0.0;
    }
    public double TotalPrice()
    {
        return SubTotal()-Promotion.CalPromotion(SubTotal());
    }
}