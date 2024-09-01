using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class MarketContext:DbContext
    {
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Storage> Stores { get; set; }
        private string _connectionString;

        public MarketContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=example;Database=Market");
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>(entity =>
            {
                entity.ToTable("ProductGroups");
                entity.HasKey(e => e.Id).HasName("ProductGroup_pkey");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Description).HasColumnName("description");

            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Product_pkey");

                entity.ToTable("Products");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("Name");
                entity.Property(e => e.Description)
                    .HasMaxLength(1024)
                    .HasColumnName("Description");

                entity.HasOne(e => e.ProductGroup).WithMany(p => p.Products);
            });



            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Store_pkey");

                entity.ToTable("Stores");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Quantity).HasColumnName("Quantity");

                entity.HasOne(e => e.Product).WithMany(p => p.Stores);
            });
        }
    }
}
