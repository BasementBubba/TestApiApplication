using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebApplication1.Crypto;
using WebApplication1.DatabaseContext;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.DatabaseContext.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private BaseContext context;
        public AccountController(BaseContext context)
        {
            this.context = context;
        }

        [HttpPost("/token")]
        public async Task<IActionResult> Login(string login, string password)
        {
            var identity = await GetIdentity(login, password, context);
            if (identity == null)
            {
                return BadRequest("There's no account with such login or password");
            }
            var jwt = new JwtSecurityToken(issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var jwt_encoded = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = jwt_encoded,
                username = identity.Name
            };

            return Ok(JsonSerializer.Serialize(response));
        }

        [HttpPost("/register"), Authorize("Admin Access")]
        public async Task<IActionResult> RegisterUser(string login, string password, string name)
        {
            var sypher = new Sypher();
            var salt = sypher.GetSalt();
            var user = new Buyer()
            {
                Login = login,
                Name = name,
                Salt = salt,
                PasswordHash = sypher.GetPasswordHash(salt, password),
                RoleId = 1
            };
            context.Buyers.Add(user);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("/changerole"), Authorize("Admin Access")]
        public async Task<IActionResult> ChangeUserRole(int user_id, int new_role_id)
        {
            var role = await context.Roles.Where(u => u.Id == new_role_id).FirstAsync();
            if (role != null)
            {
                var user = await context.Buyers.Where(u => u.Id == user_id).FirstAsync();
                if (user != null)
                {
                    user.RoleId = new_role_id;
                    await context.SaveChangesAsync();
                    return Ok();
                }
            }
            return NotFound("There's no buyers or no roles with such id");
        }

        [HttpPost("/delete_buyer"), Authorize("Admin Access")]
        public async Task<IActionResult> DeleteBuyer(int user_id)
        {
                var user = await context.Buyers.Where(u => u.Id == user_id).FirstAsync();
                if (user != null)
                {
                    context.Buyers.Remove(user);
                    await context.SaveChangesAsync();
                    return Ok();
                }
            return NotFound("There's no buyers or no roles with such id");
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password, BaseContext context)
        {
            var user = await context.Buyers.Where(b => b.Login == login).FirstAsync();
            var sypher = new Sypher();
            if (user != null || sypher.GetPasswordHash(user.Salt, password) == user.PasswordHash)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, "Token",
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return identity;
            }
            return null;
        }
    }
}
