
using System.ComponentModel.DataAnnotations;
using Internals.Models;

namespace Internals.ViewModels;

public class UserRegister
{
    [Required]
    public string? Name { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }
    [Required]
    public string? Address { get; set; }
    [Required]
    public string? PhoneNumber { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? RePassword { get; set; }

    public bool ComparePassword()
    {
        return Password != null && Password.Equals(RePassword);
    }
    
    public User ConvertToUser()
    {
        var user = new User()
        {
            Avatar = "/img/default-avatar.png",
            Name = Name,
            Address = Address,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Username = Username,
            IsBlocked = true,
            Password = Password
        };
        user.SetDateTime();
        user.HashPassword();
        return user;
    }
}