using Microsoft.EntityFrameworkCore;
using static WebApplication1.DatabaseContext.BaseContext;

namespace WebApplication1.DatabaseContext.Models
{
    [EntityTypeConfiguration(typeof(SalesPointConfigurator))]
    public class SalesPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<ProvidedProduct>? ProvidedProducts { get; set; }
    }
}
