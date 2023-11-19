namespace Internals.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public virtual ICollection<Phone>? Phones { get; set; }
    public virtual ICollection<Promotion>? Promotions { get; set; }

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
