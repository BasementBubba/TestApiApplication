using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.DatabaseContext;
using WebApplication1.DatabaseContext.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class SalesController : ControllerBase
    {
        private BaseContext context;
        public SalesController(BaseContext baseContext)
        {
            context = baseContext;
        }

        [HttpPost("/buy")]
        public async Task<IActionResult> BuyProduct(Order order)
        {
            var point = context.SalesPoints.Where(p => p.Id == order.SalesPointId)?.FirstOrDefault();
            if (point != null)
            {
                Buyer buyer = null;
                var claims = CheckIdentity();
                if (claims != null)
                {
                    var name = claims.Where(c => c.Type == ClaimsIdentity.DefaultNameClaimType).FirstOrDefault()?.Value;
                    buyer = context.Buyers.Where(b => b.Login == name).FirstOrDefault();
                }
                var sale = new Sale()
                {
                    Date = DateOnly.FromDateTime(DateTime.UtcNow),
                    Time = TimeOnly.FromDateTime(DateTime.UtcNow),
                    SalePoint = point,
                    Buyer = buyer,
                    SalesData = new List<SaleData>()
                };
                foreach (var or in order.ProductOrders)
                {
                    var p_product = point.ProvidedProducts.Where(p => p.ProductId == or.ProductId && p.ProductQuantity >= or.ProductCount);
                    if (!p_product.Any())
                    {
                        return BadRequest($"There's not enough count of {or.ProductId} product in the {point.Id} point or there's no such a product");
                    }
                    else
                    {
                        var provided_product = p_product.First();
                        var saleData = new SaleData()
                        {
                            ProductId = provided_product.ProductId,
                            ProductQuantity = or.ProductCount,
                            ProductAmount = provided_product.Product.Price * or.ProductCount
                        };
                        sale.SalesData.Add(saleData);
                        provided_product.ProductQuantity -= or.ProductCount;
                    }
                }
                foreach (var s in sale.SalesData)
                {
                    sale.TotalAmount += s.ProductAmount;
                }
                context.Sales.Add(sale);
                await context.SaveChangesAsync();
                return Ok(sale.TotalAmount);
            }
            else return BadRequest("There's no point with such an Id");
        }

        [HttpGet("/products")]
        public async Task<IActionResult> ShowProducts()
        {
            var products = context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("/sales-points")]
        public async Task<IActionResult> ShowPoints()
        {
            return Ok(context.SalesPoints.ToList());
        }
        public record Order(int SalesPointId, List<ProductOrder> ProductOrders);
        public record ProductOrder(int ProductId, int ProductCount);
        private List<Claim> CheckIdentity()
        {
            var user_identity = HttpContext.User?.Identities?.Where(c => c.Claims
                    .Where(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Any() &&
                            c.Claims.Where(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Any()).FirstOrDefault();
            if (user_identity != null)
            {
                return user_identity.Claims.ToList();
            }
            return null;
        }
    }
}
