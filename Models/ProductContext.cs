using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class ProductContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Storage> Storages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=example;Database=Market");
            //optionsBuilder.UseSqlServer("Server=CHUBICENOTEBOOK\\SQLEXPRESS;Database=Market;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(e => e.Id).HasName("ProductId");
                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Name)
                        .HasColumnName("ProductName")
                        .HasMaxLength(255);

                entity.Property(e => e.Description)
                        .HasColumnName("Description")
                        .HasMaxLength(255)
                        .IsRequired();

                entity.Property(e => e.Cost)
                        .HasColumnName("Cost")
                        .IsRequired();

                entity.HasOne(e => e.Category).WithMany(c => c.Products)
                        .HasForeignKey(e => e.Id)
                        .HasConstraintName("ProductStorage");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("ProductCategories");

                entity.HasKey(e => e.Id).HasName("CategoryId");
                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Name)
                        .HasColumnName("ProductName")
                        .HasMaxLength(255)
                        .IsRequired();

                entity.Property(e => e.Description)
                        .HasColumnName("Description")
                        .HasMaxLength(255)
                        .IsRequired();
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("Storages");

                entity.HasKey(e => e.Id).HasName("StorageId");
                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Name)
                        .HasColumnName("StorageName")
                        .HasMaxLength(255)
                        .IsRequired();

                entity.Property(e => e.Description)
                        .HasColumnName("Description")
                        .HasMaxLength(255)
                        .IsRequired();

                entity.Property(e => e.Count)
                        .HasColumnName("ProductCount");

                entity.HasMany(e => e.Products)
                        .WithMany(m => m.Storages)
                        .UsingEntity(j => j.ToTable("ProductStorage").HasNoKey());
            });
        }
    }
}
