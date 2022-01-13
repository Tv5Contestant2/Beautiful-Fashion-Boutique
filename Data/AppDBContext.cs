using ECommerce1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce1.Data
{
    public class AppDBContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDBContext(DbContextOptions<AppDBContext> context) : base(context)
        {
        }

        public DbSet<About> AboutUs { get; set; }

        public DbSet<Announcements> Announcements { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartDetails> CartDetails { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

        public DbSet<Employees> Employees { get; set; }

        public DbSet<Gender> Genders { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Modules> Modules { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Orders> Orders { get; set; }

        public DbSet<OrderDetails> OrdersDetails { get; set; }

        public DbSet<OrderShippingInfo> OrderShippingInfo { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<ProductVariant> ProductVariants { get; set; }

        public DbSet<Promos> Promos { get; set; }

        public DbSet<Returns> Returns { get; set; }

        public DbSet<RoleAccessibility> RoleAccessability { get; set; }

        //public DbSet<Roles> Roles { get; set; }

        public DbSet<Security> Security { get; set; }

        public DbSet<Size> Sizes { get; set; }

        public DbSet<SocMed> Socials { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Settings> Settings { get; set; }

        public DbSet<StockStatus> StockStatuses { get; set; }

        public DbSet<Transactions> Transactions { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}