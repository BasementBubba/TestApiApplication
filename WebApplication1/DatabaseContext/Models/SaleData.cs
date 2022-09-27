using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.DatabaseContext.Models
{
    public class SaleData
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductAmount { get; set; }
    }
}
