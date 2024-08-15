using Microsoft.EntityFrameworkCore;

namespace SharedModels.Models
{
    public class ProductContext : DbContext
    {
        private readonly string _connectionString;
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }
        public virtual DbSet<ProductStorage> ProductStorages { get; set; }

        public ProductContext() { }
        public ProductContext(string connectionString)
        {
            this._connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString).UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("Products");

                e.HasKey(x => x.ID).HasName("ID");
                e.Property(x => x.ID).HasColumnName("ID");

                e.HasIndex(x => x.Name).IsUnique();
                e.Property(x => x.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(255)
                    .IsRequired(true);
                
                e.Property(x => x.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(1023)
                    .IsRequired(false);

                e.Property(x => x.Price)
                    .HasColumnName("Price")
                    .IsRequired(true);

                e.HasOne(x => x.Category)
                    .WithMany(y => y.Products)
                    .HasForeignKey(x => x.CategoryID)
                    .HasConstraintName("CategoryToProduct");
            });


            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");

                e.HasKey(x => x.ID).HasName("CategoryID");
                e.Property(x => x.ID).HasColumnName("ID");
                
                e.HasIndex(x => x.Name).IsUnique();
                e.Property(x => x.Name)
                   .HasColumnName("Name")
                   .HasMaxLength(255)
                   .IsRequired(true);

            });


            modelBuilder.Entity<Storage>(e =>
            {
                e.ToTable("Storages");

                e.HasKey(e => e.ID).HasName("StorageID");
                e.Property(e => e.ID).HasColumnName("ID");

                e.HasIndex(x => x.Name).IsUnique();
                e.Property(x => x.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(255)
                    .IsRequired(true);

            });

            modelBuilder.Entity<ProductStorage>(e =>
            {
                e.ToTable("ProductStorages");

                e.HasKey(x => x.ID).HasName("ProductStorageID");
                e.Property(x => x.ID).HasColumnName("ID");

                e.Property(x => x.ProductID)
                    .HasColumnName("ProductID")
                    .IsRequired(true);
                
                e.Property(x => x.StorageID)
                    .HasColumnName("StorageID")
                    .IsRequired(true);

                //e.HasOne(x => x.Product)
                    //.WithMany(y => y.ProductStorages)
                    //.HasForeignKey(x => x.ProductID)
                    //.HasConstraintName("ProductToProductStorage");

                e.HasOne(x => x.Storage).WithMany(y => y.ProductStorages)
                    .HasForeignKey(x => x.StorageID)
                    .HasConstraintName("StorageToProductStorage");

            });
        }
    }
}
