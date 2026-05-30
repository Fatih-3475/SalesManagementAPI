using Microsoft.EntityFrameworkCore;
using SalesManagementAPI.Core.Entities;

namespace SalesManagementAPI.DataAccess.Contexts;

public class SalesManagementDbContext : DbContext
{
    public SalesManagementDbContext(DbContextOptions<SalesManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.Phone)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(x => x.CreatedDate)
                  .IsRequired();

            entity.HasIndex(x => x.Email)
            .IsUnique();

            entity.HasIndex(x => x.Phone)
            .IsUnique();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(x => x.Price)
                  .HasColumnType("numeric(18,2)");

            entity.Property(x => x.Stock)
                  .IsRequired();

            entity.HasIndex(x => x.Name)
            .IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrderDate)
                  .IsRequired();

            entity.Property(x => x.TotalAmount)
                  .HasColumnType("numeric(18,2)");

            entity.HasOne(x => x.Customer)
                  .WithMany(x => x.Orders)
                  .HasForeignKey(x => x.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Quantity)
                  .IsRequired();

            entity.Property(x => x.UnitPrice)
                  .HasColumnType("numeric(18,2)");

            entity.HasOne(x => x.Order)
                  .WithMany(x => x.OrderItems)
                  .HasForeignKey(x => x.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Product)
                  .WithMany(x => x.OrderItems)
                  .HasForeignKey(x => x.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}