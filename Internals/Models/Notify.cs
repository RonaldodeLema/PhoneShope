namespace Internals.Models;

public class Notify
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Data;
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }    
    
}