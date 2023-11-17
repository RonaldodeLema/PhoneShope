using System.Security.Cryptography;
using System.Text;

namespace Internals.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? DeviceToken { get; set; }
        public string Avatar { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<Promotion>? Promotions { get; set; }

        public void SetDateTime()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public void SetUpdateDate()
        {
            UpdatedAt = DateTime.Now;
        }
        public void HashPassword()
        {
            using var hasher = SHA256.Create();            var hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(Password));
            Password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
        
        public bool ComparePasswords(string password)
        {
            using var hasher = SHA256.Create();
            var hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return Password.Equals(hashedPassword);
        }

        public void SanitizePassword()
        {
            Password = "";
        }
    }
}