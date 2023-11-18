using System.Text;

namespace Internals.ViewModels;

public class ResetPasswordModel
{
    public string NewPassword { get; set; }
    public string ReNewPassword { get; set;}
    public string ResetToken { get; set; }
    public string Email { get; set; }
    [Obsolete("Obsolete")]
    public void HashPassword()
    {
        using var hasher = new System.Security.Cryptography.SHA256Managed();
        byte[] hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(this.NewPassword));
        this.NewPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        this.ReNewPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}