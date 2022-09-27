using Microsoft.EntityFrameworkCore;
using static WebApplication1.DatabaseContext.BaseContext;

namespace WebApplication1.DatabaseContext.Models
{
    [EntityTypeConfiguration(typeof(RoleConfigurator))]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Buyer>? Buyers { get; set; }
    }
}
