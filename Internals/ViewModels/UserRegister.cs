
using Internals.Models;

namespace Internals.ViewModels;

public class UserRegister
{
    public string? Name { get; set; }
    public string? Email { get; set; }
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
            Name = this.Name,
            Email = this.Email,
            Username = this.Username,
            Password = this.Password
        };
        user.HashPassword();
        return user;
    }
}