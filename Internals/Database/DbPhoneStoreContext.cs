using Internals.Models;
using Microsoft.EntityFrameworkCore;

namespace Internals.Database;

public class DbPhoneStoreContext: DbContext
{

    public DbPhoneStoreContext(DbContextOptions<DbPhoneStoreContext> options) : base(options)
    {
    }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<PhoneDetails> PhoneDetails { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Notify> Notifications { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }
    public DbSet<Payment> Payments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Admin>().ToTable("Admin");
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Category>().ToTable("Category");
        modelBuilder.Entity<Phone>().ToTable("Phone");
        modelBuilder.Entity<PhoneDetails>().ToTable("PhoneDetails");
        modelBuilder.Entity<Order>().ToTable("Order");
        modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
        modelBuilder.Entity<Promotion>().ToTable("Promotion");
        modelBuilder.Entity<Notify>().ToTable("Notification");
        modelBuilder.Entity<Image>().ToTable("Image");
        modelBuilder.Entity<Role>().ToTable("Role");
        modelBuilder.Entity<RoleClaim>().ToTable("RoleClaim");
        modelBuilder.Entity<Payment>().ToTable("Payment");
    }
}