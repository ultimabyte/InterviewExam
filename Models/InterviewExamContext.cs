using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class InterviewExamContext : DbContext
    {
        public InterviewExamContext()
        {
        }

        public InterviewExamContext(DbContextOptions<InterviewExamContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MstProductCategory> MstProductCategories { get; set; }
        public virtual DbSet<TOrder> TOrders { get; set; }
        public virtual DbSet<TOrderItem> TOrderItems { get; set; }
        public virtual DbSet<TProduct> TProducts { get; set; }
        public virtual DbSet<TProductInfo> TProductInfos { get; set; }
        public virtual DbSet<TProductItem> TProductItems { get; set; }
        public virtual DbSet<TShoppingCart> TShoppingCarts { get; set; }
        public virtual DbSet<TShoppingItem> TShoppingItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                          .AddJsonFile("appsettings.json")
                          .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<MstProductCategory>(entity =>
            {
                entity.ToTable("Mst_ProductCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TOrder>(entity =>
            {
                entity.ToTable("T_Order");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ShoppingCart)
                    .WithMany(p => p.TOrders)
                    .HasForeignKey(d => d.ShoppingCartId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_T_Order_T_ShoppingCart");
            });

            modelBuilder.Entity<TOrderItem>(entity =>
            {
                entity.ToTable("T_OrderItem");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_T_OrderItem_T_Order");

                entity.HasOne(d => d.ShoppingItem)
                    .WithMany(p => p.TOrderItems)
                    .HasForeignKey(d => d.ShoppingItemId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_T_OrderItem_T_ShoppingItem");
            });

            modelBuilder.Entity<TProduct>(entity =>
            {
                entity.ToTable("T_Product");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SKU");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TProducts)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Mst_Product_Mst_Category");
            });

            modelBuilder.Entity<TProductInfo>(entity =>
            {
                entity.ToTable("T_ProductInfo");

                entity.Property(e => e.Detail)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.EffectDate).HasColumnType("date");

                entity.Property(e => e.Image1400x400)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Image1_400x400");

                entity.Property(e => e.Image2400x400)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Image2_400x400");

                entity.Property(e => e.Image3400x400)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Image3_400x400");

                entity.Property(e => e.Image4400x400)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Image4_400x400");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TProductInfos)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_T_ProductInfo_Mst_Product");
            });

            modelBuilder.Entity<TProductItem>(entity =>
            {
                entity.ToTable("T_ProductItem");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateIn).HasColumnType("date");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TProductItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_T_ProductItem_Mst_Product");
            });

            modelBuilder.Entity<TShoppingCart>(entity =>
            {
                entity.ToTable("T_ShoppingCart");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TShoppingItem>(entity =>
            {
                entity.ToTable("T_ShoppingItem");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TShoppingItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_T_ShoppingItem_Mst_Product");

                entity.HasOne(d => d.ShoppingCart)
                    .WithMany(p => p.TShoppingItems)
                    .HasForeignKey(d => d.ShoppingCartId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_T_ShoppingItem_T_ShoppingCart");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
