using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApplication1.Crypto
{
    public class AuthOptions
    {
        public const string Issuer = "BIG SMOKE API Issuer";
        public const string Audience = "BIG SMOKE API Client";
        public const int Lifetime = 1;
        private const string Key = "HeresThisApp_Security!Key";
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
