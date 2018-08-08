using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StoreReactNET.Models.Database
{
    public partial class StoreASPContext : DbContext
    {
        public StoreASPContext()
        {
        }

        public StoreASPContext(DbContextOptions<StoreASPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<ProductCategories> ProductCategories { get; set; }
        public virtual DbSet<ProductDetails> ProductDetails { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<UserAdresses> UserAdresses { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\v14.0;Initial Catalog=StoreASP;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItems>(entity =>
            {
                entity.HasIndex(e => e.CartId)
                    .HasName("IX_FK_CartCartItem");

                entity.HasIndex(e => e.ProductId)
                    .HasName("IX_FK_ProductCartItem");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasColumnName("quantity");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartCartItem");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCartItem");
            });

            modelBuilder.Entity<Carts>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_FK_UserCart");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCart");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasIndex(e => e.CartId)
                    .HasName("IX_FK_CartOrder");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_CartOrder");
            });

            modelBuilder.Entity<ProductCategories>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryName).IsRequired();
            });

            modelBuilder.Entity<ProductDetails>(entity =>
            {
                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.CoreBaseClockMhz).HasColumnName("CoreBaseClockMHZ");

                entity.Property(e => e.CoreBoostClockMhz).HasColumnName("CoreBoostClockMHZ");

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Vram).HasColumnName("VRAM");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductDetails_Products");
            });

            modelBuilder.Entity<ProductImages>(entity =>
            {
                entity.HasIndex(e => e.ProductId)
                    .HasName("IX_FK_ProductProductImage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImageName).IsRequired();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductProductImage");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasIndex(e => e.ProductCategoryId)
                    .HasName("IX_FK_ProductProductsCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PriceVat).HasColumnName("priceVAT");

                entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");

                entity.Property(e => e.ProductDetailsId).HasColumnName("ProductDetailsID");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductProductsCategory");

                entity.HasOne(d => d.ProductDetailsNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductDetailsId)
                    .HasConstraintName("FK_ProductsProductDetails");
            });

            modelBuilder.Entity<UserAdresses>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppartmentNr)
                    .IsRequired()
                    .HasColumnName("AppartmentNR");

                entity.Property(e => e.City).IsRequired();

                entity.Property(e => e.Country).IsRequired();

                entity.Property(e => e.HomeNr)
                    .IsRequired()
                    .HasColumnName("HomeNR");

                entity.Property(e => e.StreetName).IsRequired();

                entity.Property(e => e.Zipcode).IsRequired();
            });

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateOfBirth).IsRequired();

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.UserAdressId)
                    .HasName("IX_FK_UserUserAdress");

                entity.HasIndex(e => e.UserDetailsId)
                    .HasName("IX_FK_UserDetailsUser");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(256);

                entity.Property(e => e.UserAdressId).HasColumnName("UserAdressID");

                entity.Property(e => e.UserDetailsId).HasColumnName("UserDetailsID");

                entity.HasOne(d => d.UserAdress)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserAdressId)
                    .HasConstraintName("FK_UserUserAdress");

                entity.HasOne(d => d.UserDetails)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserDetailsId)
                    .HasConstraintName("FK_UserDetailsUser");
            });
        }
    }
}
