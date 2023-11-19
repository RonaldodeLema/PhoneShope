using System.ComponentModel.DataAnnotations;

namespace Internals.ViewModels;

public class UserUpdate
{
    public string? Name { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }
    [Required]
    public string? Address { get; set; }
    [Required]
    public string? PhoneNumber { get; set; }
}