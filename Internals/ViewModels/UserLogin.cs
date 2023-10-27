using System.Security.Cryptography;
using System.Text;

namespace Internals.ViewModels
{
    public class UserLogin
    {
        private static readonly byte[] Key = Convert.FromBase64String("bDT2WXsSoU8HVddBXNtDAhaGFguEl+VKrmOuLKEw3Qc=");
        private static readonly byte[] IV = Convert.FromBase64String("8j3xDjyAuOVG4Gh7MKvy9Q==");
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public void HashPassword()
        {
            using var hasher = SHA256.Create();
            var hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(Password));
            Password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
        public string EncryptPassword()
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(Password);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
    }
}
