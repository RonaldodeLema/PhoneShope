using Internals.Models;
using Internals.Models.Enum;

namespace Internals.ViewModels;

public class AdminRegister
{
    public string? Username { get; set; }
    public string RePassword { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public bool ComparePassword()
    {
        return RePassword.Equals(Password);
    }
    
    public Admin ConvertToAdmin()
    {
        var role = (Role)Role;
        var user = new Admin()
        {
            Username = Username,
            Password = Password,
            Role = role,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        user.HashPassword();
        return user;
    }
}