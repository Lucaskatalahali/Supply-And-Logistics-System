using Microsoft.EntityFrameworkCore;
using Supply_And_Logistics_System.Models.Cart;
using Supply_And_Logistics_System.Models.Identity;
using Supply_And_Logistics_System.Models.Notifications;
using Supply_And_Logistics_System.Models.Orders;
using Supply_And_Logistics_System.Models.Products;
using Supply_And_Logistics_System.Models.Shipping;

namespace Supply_And_Logistics_System.Data
{
    /// <summary>
    /// Uygulamanın veritabanı bağlam sınıfı.
    /// Entity Framework Core kullanarak modellerin veritabanı tablolarına eşlenmesini sağlar.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // --- Temel Tablolar ---
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // --- Composite Pattern Modelleri ---
        public DbSet<SimpleProduct> SimpleProducts { get; set; }
        public DbSet<CompositeProduct> CompositeProducts { get; set; }

        // --- Alışveriş ve Lojistik Tabloları ---
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<StockNotification> StockNotifications { get; set; }

        // Veritabanı modelleri oluşturulurken ilişkileri ve kalıtım stratejilerini yapılandırır.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================================
            // PRODUCT TPH (Table-Per-Hierarchy) YAPILANDIRMASI
            // Composite Pattern'deki hiyerarşiyi tek bir tabloda saklar.
            // ============================================================
            modelBuilder.Entity<Product>()
                .HasDiscriminator<string>("ProductType")
                .HasValue<SimpleProduct>("Simple")
                .HasValue<CompositeProduct>("Composite");

            // ============================================================
            // SİPARİŞ (ORDER) YAPILANDIRMASI
            // ============================================================
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Sipariş silinirse kalemleri de silinir.

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================================================
            // SEPET (CART) YAPILANDIRMASI
            // ============================================================
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany() 
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Composite Product'ın alt bileşenleri ile olan ilişkisi
            modelBuilder.Entity<CompositeProduct>()
                .HasMany(cp => cp.Components)
                .WithMany();
        }
    }
}