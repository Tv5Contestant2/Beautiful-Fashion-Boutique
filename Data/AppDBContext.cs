using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce1.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> context) : base(context) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<About>(
                ab =>
                {
                    ab.HasNoKey();
                    ab.ToView("View_About");
                });
            modelBuilder
                .Entity<SocMed>(
                sm =>
                {
                    sm.HasNoKey();
                    sm.ToView("View_SocMed");
                });
        }

        public DbSet<About> AboutUs { get; set; }
        public DbSet<Announcements> Announcements { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<OrderDetails> OrdersDetails { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promos> Promos { get; set; }
        public DbSet<RoleAccessibility> RoleAccessability { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Security> Security { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<SocMed> Socials { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}
