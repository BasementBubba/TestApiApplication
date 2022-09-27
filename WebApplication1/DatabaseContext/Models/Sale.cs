using Microsoft.EntityFrameworkCore;
using static WebApplication1.DatabaseContext.BaseContext;

namespace WebApplication1.DatabaseContext.Models
{
    [EntityTypeConfiguration(typeof(SaleConfigurator))]
    public class Sale
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int SalePointId { get; set; }
        public virtual SalesPoint SalePoint { get; set; }
        public int? BuyerId { get; set; }
        public virtual Buyer? Buyer { get; set; }
        public virtual List<SaleData> SalesData { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
