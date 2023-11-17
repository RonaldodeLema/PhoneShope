using System.Text;
using Internals.Models.Enum;

namespace Internals.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Role Role { get; set; }

        public void HashPassword()
        {
            using var hasher = new System.Security.Cryptography.SHA256Managed();
            byte[] hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(this.Password));
            this.Password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
        

        public bool ComparePasswords(string password)
        {
            using var hasher = new System.Security.Cryptography.SHA256Managed();
            byte[] hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return this.Password.Equals(hashedPassword);
        }
    
        public void SanitizePassword()
        {
            this.Password = "";
        }
    }
}