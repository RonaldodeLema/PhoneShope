namespace Internals.Models;

public class Notify
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public int UserId { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Data;
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }    
    public void SetDateTime()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    public void SetUpdateDate()
    {
        UpdatedAt = DateTime.Now;
    }

    public void SetActionBy(string? username)
    {
        CreatedBy = username;
        UpdatedBy = username;
    }
    public void SetUpdateBy(string? username)
    {
        UpdatedBy = username;
    }
    
}