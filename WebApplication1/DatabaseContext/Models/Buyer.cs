using Microsoft.EntityFrameworkCore;
using static WebApplication1.DatabaseContext.BaseContext;

namespace WebApplication1.DatabaseContext.Models
{
    [EntityTypeConfiguration(typeof(BuyerConfigurator))]
    public class Buyer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Sale>? Sales { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
