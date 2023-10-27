
using Internals.Models;

namespace Internals.ViewModels;

public class UserRegister
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
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
            IsBlocked = false,
            Password = Password
        };
        user.SetDateTime();
        user.HashPassword();
        return user;
    }
}