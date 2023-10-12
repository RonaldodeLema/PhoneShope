using Internals.Models;
using Internals.Models.Enum;

namespace Internals.ViewModels;

public class AdminRegister
{
    public string? Username { get; set; }
    public string RePassword { get; set; }
    public int RoleId { get; set; }
    public string Password { get; set; }
    public bool ComparePassword()
    {
        return RePassword.Equals(Password);
    }
    
    public Admin ConvertToAdmin()
    {
        var user = new Admin()
        {
            Username = Username,
            Password = Password,
            RoleId = RoleId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        user.HashPassword();
        return user;
    }
}