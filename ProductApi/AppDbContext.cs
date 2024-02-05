using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductApi.Models.Staging;

namespace ProductApi
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<StgInventory> StgInventories { get; set; }
        public DbSet<StgPrices> StgPrices { get; set; }
        public DbSet<StgProduct> StgProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Inventory)
            .WithOne(i => i.Product)
            .HasForeignKey<Inventory>(i => i.ProductId);

            modelBuilder.Entity<Product>()
            .HasOne(prod => prod.Price)
            .WithOne(p => p.Product)
            .HasPrincipalKey<Product>(prod => prod.SKU)
            .HasForeignKey<Price>(p => p.ProductSKU);
        }
    }
}