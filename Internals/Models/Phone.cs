using Internals.Models.Enum;

namespace Internals.Models;

public class Phone
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public BrandPhone Brand { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Category Category { get; set; }
    public  ICollection<PhoneDetails>? PhoneDetails { get; set; }
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
}
