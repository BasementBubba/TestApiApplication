using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using static WebApplication1.DatabaseContext.BaseContext;

namespace WebApplication1.DatabaseContext.Models
{
    [EntityTypeConfiguration(typeof(ProvidedConfigurator))]
    public class ProvidedProduct
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductQuantity { get; set; }
        [JsonIgnore]
        public int SalesPointId { get; set; }
        [JsonIgnore]
        public virtual SalesPoint SalesPoint { get; set; }
    }
}
