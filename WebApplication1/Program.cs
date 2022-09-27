using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DatabaseContext;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Crypto;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(options => 
options.AddPolicy("Admin Access",
    policy=>
    {
        policy.RequireAuthenticatedUser()
        .RequireClaim(ClaimsIdentity.DefaultRoleClaimType, "Admin");
    }));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = AuthOptions.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };
});
// при проверке не забыть указать свои параметры в строке подключения
string connectionString = builder.Configuration.GetConnectionString("PostgreSQLConn");
builder.Services.AddDbContext<BaseContext>(options => options.UseLazyLoadingProxies().UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
