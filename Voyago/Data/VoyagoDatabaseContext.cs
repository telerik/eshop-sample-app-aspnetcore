using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data
{
    public partial class VoyagoDatabaseContext : DbContext
    {
        public VoyagoDatabaseContext()
        {
        }

        public VoyagoDatabaseContext(DbContextOptions<VoyagoDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<SalesOrder> SalesOrders { get; set; } = null!;
        public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=VoyagoDatabase;Trusted_Connection=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.ContactId)
                    .HasColumnName("ContactID")
                    .HasComment("Primary key for Contact records.");

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.EmailAddress).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Phone).HasMaxLength(25);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.Street).HasMaxLength(60);

                entity.Property(e => e.ZipCode).HasMaxLength(15);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProductID")
                    .HasComment("Primary key for Product records.");

                entity.Property(e => e.Color).HasMaxLength(15);

                entity.Property(e => e.Description).HasMaxLength(400);

                entity.Property(e => e.DiscountPct).HasColumnType("smallmoney");

                entity.Property(e => e.ListPrice).HasColumnType("money");

                entity.Property(e => e.PhotoId).HasColumnName("PhotoID");

                entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategoryID");

                entity.Property(e => e.ProductCategoryName).HasMaxLength(50);

                entity.Property(e => e.ProductModel).HasMaxLength(50);

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ProductNumber).HasMaxLength(25);

                entity.Property(e => e.ProductSubCategoryName).HasMaxLength(50);

                entity.Property(e => e.ProductSubcategoryId).HasColumnName("ProductSubcategoryID");

                entity.Property(e => e.Size).HasMaxLength(5);

                entity.Property(e => e.Weight).HasColumnType("decimal(8, 2)");
            });

            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("SalesOrder");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasComment("Primary key.");

                entity.Property(e => e.OrderNumber).HasColumnType("int");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.LineTotal).HasColumnType("numeric(38, 6)");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.SalesOrders)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SalesOrders)
                    .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.ToTable("ShoppingCartItem");

                entity.Property(e => e.ShoppingCartItemId).HasColumnName("ShoppingCartItemID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ShoppingCartId)
                    .HasMaxLength(50)
                    .HasColumnName("ShoppingCartID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCartItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
