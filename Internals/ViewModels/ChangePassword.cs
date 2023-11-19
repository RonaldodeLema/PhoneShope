using System.Text;

namespace Internals.ViewModels
{
    public class ChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ReNewPassword { get; set;}
        
        public bool CompareNewPassword()
        { 
            return this.NewPassword.Equals(this.ReNewPassword);
        }
        public bool CompareOldPassword()
        { 
            return this.OldPassword.Equals(this.NewPassword);
        }
        public void HashPassword()
        {
            using (var hasher = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(this.NewPassword));
                this.NewPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                this.ReNewPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}