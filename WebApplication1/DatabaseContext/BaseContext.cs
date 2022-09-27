using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Crypto;
using WebApplication1.DatabaseContext.Models;

namespace WebApplication1.DatabaseContext
{
    public class BaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProvidedProduct> ProvidedProducts { get; set; }
        public DbSet<SalesPoint> SalesPoints { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public class SalesPointConfigurator : IEntityTypeConfiguration<SalesPoint>
        {
            public void Configure(EntityTypeBuilder<SalesPoint> builder)
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
            }
        }

        public class BuyerConfigurator : IEntityTypeConfiguration<Buyer>
        {
            public void Configure(EntityTypeBuilder<Buyer> builder)
            {
                builder.HasKey(a => a.Id);
                builder.HasAlternateKey(a => a.Login);
                builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
                builder.Property(a => a.Salt).IsRequired().HasMaxLength(20);
                builder.Property(a => a.Login).IsRequired().HasMaxLength(20);
                builder.Property(a => a.RoleId).IsRequired();
                builder.Property(a => a.PasswordHash).IsRequired().HasMaxLength(200);
            }
        }

        public class ProductConfigurator : IEntityTypeConfiguration<Product>
        {
            public void Configure(EntityTypeBuilder<Product> builder)
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.Name).
                    HasMaxLength(50).IsRequired();
                builder.Property(a => a.Price).IsRequired();
            }
        }

        public class SaleConfigurator : IEntityTypeConfiguration<Sale>
        {
            public void Configure(EntityTypeBuilder<Sale> builder)
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.Time).IsRequired();
                builder.Property(a => a.Date).IsRequired();
                builder.Property(a => a.TotalAmount).IsRequired();
            }
        }

        public class ProvidedConfigurator : IEntityTypeConfiguration<ProvidedProduct>
        {
            public void Configure(EntityTypeBuilder<ProvidedProduct> builder)
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.ProductQuantity).IsRequired();
            }
        }

        public class RoleConfigurator : IEntityTypeConfiguration<Role>
        {
            public void Configure(EntityTypeBuilder<Role> builder)
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.Name).HasMaxLength(15).IsRequired();
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var IPhone_12 = new Product() { Id = 1, Name = "IPhone 12", Price = 60000 };
            var IPhone_12S = new Product() { Id = 2, Name = "IPhone 12S", Price = 70000 };
            var IPhone_13 = new Product() { Id = 3, Name = "IPhone 13", Price = 80000 };
            var IPhone_13S = new Product() { Id = 4, Name = "IPhone 13S", Price = 90000 };
            var IPhone_14 = new Product() { Id = 5, Name = "IPhone 14", Price = 120000 };
            var IPhone_14S = new Product() { Id = 6, Name = "IPhone 14S", Price = 140000 };
            var Samsung_Galaxy_A = new Product() { Id = 7, Name = "Samsung Galaxy A", Price = 65000 };
            var Samsung_Galaxy_B = new Product() { Id = 8, Name = "Samsung Galaxy B", Price = 75000 };
            var Samsung_Galaxy_C = new Product() { Id = 9, Name = "Samsung Galaxy C", Price = 85000 };
            var Samsung_Galaxy_D = new Product() { Id = 10, Name = "Samsung Galaxy D", Price = 95000 };
            var Samsung_Galaxy_E = new Product() { Id = 11, Name = "Samsung Galaxy E", Price = 105000 };

            var Point1 = new SalesPoint()
            {
                Id = 1,
                Name = "DNS"
            };
            var Point2 = new SalesPoint()
            {
                Id = 2,
                Name = "MVideo"
            };

            var roleBuyer = new Role() { Id = 1, Name = "Buyer" };
            var roleAdmin = new Role() { Id = 2, Name = "Admin" };

            var sypher = new Sypher();
            var salt1 = sypher.GetSalt();
            var salt2 = sypher.GetSalt();
            var salt3 = sypher.GetSalt();
            var salt4 = sypher.GetSalt();

            var Buyer1 = new Buyer()
            {
                Id = 1,
                Name = "James Cameron",
                Sales = null,
                RoleId = roleBuyer.Id,
                Login = "jCameron",
                Salt = salt1,
                PasswordHash = sypher.GetPasswordHash(salt1, "IAmJamesCameron")
            };
            var Buyer2 = new Buyer()
            {
                Id = 2,
                Name = "Brad Pitt",
                Sales = null,
                RoleId = roleBuyer.Id,
                Login = "bPitt",
                Salt = salt2,
                PasswordHash = sypher.GetPasswordHash(salt2, "IAmBradPitt")
            };
            var Buyer3 = new Buyer()
            {
                Id = 3,
                Name = "Angelina Jolie",
                Sales = null,
                RoleId = roleBuyer.Id,
                Login = "aJolie",
                Salt = salt3,
                PasswordHash = sypher.GetPasswordHash(salt3, "IAmAngelinaJolie")
            };
            var Buyer4 = new Buyer()
            {
                Id = 4,
                Name = "Big Smoke",
                Sales = null,
                RoleId = roleAdmin.Id,
                Login = "bSmoke",
                Salt = salt4,
                PasswordHash = sypher.GetPasswordHash(salt4, "FollowTheTrainCJ")
            };

            builder.Entity<Product>().HasData(IPhone_12, IPhone_12S, IPhone_13, IPhone_13S, IPhone_14, IPhone_14S,
            Samsung_Galaxy_A, Samsung_Galaxy_B, Samsung_Galaxy_C, Samsung_Galaxy_D, Samsung_Galaxy_E);
            builder.Entity<SalesPoint>().HasData(Point1, Point2);
            builder.Entity<ProvidedProduct>().HasData(
                        new ProvidedProduct() { Id = 1, SalesPointId = Point1.Id, ProductId = IPhone_12.Id, ProductQuantity = 10 },
                        new ProvidedProduct() { Id = 2, SalesPointId = Point1.Id, ProductId = IPhone_12S.Id, ProductQuantity = 20 },
                        new ProvidedProduct() { Id = 3, SalesPointId = Point1.Id, ProductId = IPhone_13.Id, ProductQuantity = 30 },
                        new ProvidedProduct() { Id = 4, SalesPointId = Point1.Id, ProductId = IPhone_13S.Id, ProductQuantity = 40 },
                        new ProvidedProduct() { Id = 5, SalesPointId = Point1.Id, ProductId = IPhone_14.Id, ProductQuantity = 50 },
                        new ProvidedProduct() { Id = 6, SalesPointId = Point1.Id, ProductId = IPhone_14S.Id, ProductQuantity = 60 },
                        new ProvidedProduct() { Id = 7, SalesPointId = Point1.Id, ProductId = Samsung_Galaxy_A.Id, ProductQuantity = 10 },
                        new ProvidedProduct() { Id = 8, SalesPointId = Point1.Id, ProductId = Samsung_Galaxy_B.Id, ProductQuantity = 20 },
                        new ProvidedProduct() { Id = 9, SalesPointId = Point1.Id, ProductId = Samsung_Galaxy_C.Id, ProductQuantity = 30 },
                        new ProvidedProduct() { Id = 10, SalesPointId = Point1.Id, ProductId = Samsung_Galaxy_D.Id, ProductQuantity = 40 },
                        new ProvidedProduct() { Id = 11, SalesPointId = Point1.Id, ProductId = Samsung_Galaxy_E.Id, ProductQuantity = 50 },
                        new ProvidedProduct() { Id = 12, SalesPointId = Point2.Id, ProductId = IPhone_12.Id, ProductQuantity = 15 },
                        new ProvidedProduct() { Id = 13, SalesPointId = Point2.Id, ProductId = IPhone_12S.Id, ProductQuantity = 25 },
                        new ProvidedProduct() { Id = 14, SalesPointId = Point2.Id, ProductId = IPhone_13.Id, ProductQuantity = 35 },
                        new ProvidedProduct() { Id = 15, SalesPointId = Point2.Id, ProductId = IPhone_13S.Id, ProductQuantity = 45 },
                        new ProvidedProduct() { Id = 16, SalesPointId = Point2.Id, ProductId = IPhone_14.Id, ProductQuantity = 55 },
                        new ProvidedProduct() { Id = 17, SalesPointId = Point2.Id, ProductId = IPhone_14S.Id, ProductQuantity = 65 },
                        new ProvidedProduct() { Id = 18, SalesPointId = Point2.Id, ProductId = Samsung_Galaxy_A.Id, ProductQuantity = 15 },
                        new ProvidedProduct() { Id = 19, SalesPointId = Point2.Id, ProductId = Samsung_Galaxy_B.Id, ProductQuantity = 25 },
                        new ProvidedProduct() { Id = 20, SalesPointId = Point2.Id, ProductId = Samsung_Galaxy_C.Id, ProductQuantity = 35 },
                        new ProvidedProduct() { Id = 21, SalesPointId = Point2.Id, ProductId = Samsung_Galaxy_D.Id, ProductQuantity = 45 },
                        new ProvidedProduct() { Id = 22, SalesPointId = Point2.Id, ProductId = Samsung_Galaxy_E.Id, ProductQuantity = 55 });
            builder.Entity<Role>().HasData(roleBuyer, roleAdmin);
            builder.Entity<Buyer>().HasData(Buyer1, Buyer2, Buyer3, Buyer4);
        }
    }
}
