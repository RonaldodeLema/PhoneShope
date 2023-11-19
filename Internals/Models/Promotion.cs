namespace Internals.Models;

public class Promotion
{
    public int Id { get; set; }    
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string Code { get; set; }
    public bool IsUsed { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double MinTotal { get; set; }
    public double MaxReduce { get; set; }
    public double Percentage { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Category Category { get; set; }
    public User User { get; set; }

    public bool AcceptPromo(double currentTotal)
    {
        return currentTotal >= MinTotal;
    }
    public double CalPromotion(double currentTotal)
    {
        if (!AcceptPromo(currentTotal)) return currentTotal;
        if (currentTotal * Percentage >= MaxReduce)
        {
            return currentTotal - MaxReduce;
        }
        return currentTotal - (currentTotal * Percentage);
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
}
