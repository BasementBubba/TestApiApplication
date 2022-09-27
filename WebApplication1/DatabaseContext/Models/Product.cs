using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using static WebApplication1.DatabaseContext.BaseContext;

namespace WebApplication1.DatabaseContext.Models
{
    [EntityTypeConfiguration(typeof(ProductConfigurator))]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public virtual List<ProvidedProduct>? ProvidedProducts { get; set; }

    }
}
