using Internals.Models.Enum;

namespace Internals.Models;

public class PhoneDetails
{
    public int Id { get; set; }
    
    public int PhoneId { get; set; }
    public string Image { get; set; }
    public Size Size { get; set; }
    public Color Color { get; set; }
    public RAM RAM { get; set; }
    public Storage Storage { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Phone Phone { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool OutOfStock()
    {
        return Quantity == 0;
    }

    public double CalPrice(int quantity)
    {
        return quantity * Price;
    }
    public void SetDateTime()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    public void SetUpdateDate()
    {
        UpdatedAt = DateTime.Now;
    }

    public void SetActionBy(string username)
    {
        CreatedBy = username;
        UpdatedBy = username;
    }
    public void SetUpdateBy(string username)
    {
        UpdatedBy = username;
    }
    public string GetInformation()
    {
        var info = "Color: "+Color + ", Storage: " + Storage + ", RAM: " + RAM + ", Size:" + Size.ToString().Replace("p",".");
        return info.Replace("_", " ").Trim();
    }
    
}