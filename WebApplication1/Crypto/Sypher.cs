using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Crypto
{
    public class Sypher
    {
        public string GetSalt()
        {
            StringBuilder builder = new StringBuilder();
            var rand = new Random();
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();
            for (int i = 0; i < 10; i++)
            {
                builder.Append(chars[rand.Next(chars.Length - 1)]);
            }
            return builder.ToString();
        }

        public string GetPasswordHash(string salt, string password)
        {
            MD5 md5 = MD5.Create();
            var sha = SHA256.Create();
            var saltedPassword = salt + password;
            return Convert.ToBase64String(md5.ComputeHash(
                sha.ComputeHash(
                    md5.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword)))));
        }
    }
}
