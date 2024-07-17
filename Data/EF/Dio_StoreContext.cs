using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Data.EF.Entities;
using System.Linq;

namespace Data.EF
{
	public partial class Dio_StoreContext : IdentityDbContext<Account, Role, string>
	{
		public Dio_StoreContext(DbContextOptions<Dio_StoreContext> options)
			: base(options)
		{
		}
		public virtual DbSet<Account> Accounts { get; set; } = null!;
		public virtual DbSet<Category> Categories { get; set; } = null!;
		public virtual DbSet<Order> Orders { get; set; } = null!;
		public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
		public virtual DbSet<Product> Products { get; set; } = null!;
		public virtual DbSet<ProductImages> ProductImages { get; set; } = null!;
		public virtual DbSet<Rating> Ratings { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=DIO-BRANDO\\SQLEXPRESS;Database=Dio_Store;Trusted_Connection=True;");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Ánh xạ bảng Accounts vào IdentityUser
			modelBuilder.Entity<Account>(entity =>
			{
				entity.ToTable("Accounts");
				entity.Property(e => e.Id).HasColumnName("AccountID");
				entity.Property(e => e.AccountAddress).HasMaxLength(255);
				entity.Property(e => e.Email).HasColumnName("AccountEmail").HasMaxLength(100);
				entity.Property(e => e.AccountBirthday).HasColumnType("datetime");
				entity.Property(e => e.PhoneNumber).HasColumnName("AccountPhoneNumber").HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.RoleId).HasColumnName("RoleId");

				entity.HasOne(e => e.Role)
					.WithMany(r => r.Accounts)
					.HasForeignKey(e => e.RoleId);
				entity.HasOne(e=>e.Cart)
						.WithOne(c=>c.Account)
					 .HasForeignKey<Cart>(c => c.CustomerId);
			});

			// Ánh xạ bảng Roles vào IdentityRole
			modelBuilder.Entity<Role>(entity =>
			{
				entity.ToTable("Roles");
				entity.Property(e => e.Id).HasColumnName("RoleId").ValueGeneratedNever();
			});

			// Các cấu hình khác
			modelBuilder.Entity<Category>(entity =>
			{
				entity.Property(e => e.CategoryId).HasColumnName("CategoryID").ValueGeneratedOnAdd();
				entity.Property(e => e.CategoryName).HasMaxLength(100);
			});
			modelBuilder.Entity<Category>().HasData(
		   new Category { CategoryId = 1, CategoryName = "Dụng cụ cá nhân" },
		   new Category { CategoryId = 2, CategoryName = "Chăm sóc quần áo" },
		   new Category { CategoryId = 3, CategoryName = "Dụng cụ nấu bếp" },
		   new Category { CategoryId = 4, CategoryName = "Dụng cụ ăn uống" }
	   );
			modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).HasColumnName("OrderID")
											.ValueGeneratedOnAdd();
				entity.Property(e => e.AccountID).HasColumnName("AccountID")
													.HasMaxLength(450) ;
				
				entity.Property(e => e.OrderDate).HasColumnType("datetime");
				entity.Property(e => e.OrderStatus).HasMaxLength(100);
                entity.Property(e => e.PaymentMethod).HasMaxLength(200);
                entity.Property(e => e.ShipName).HasMaxLength(200);
				entity.Property(e => e.ShipEmail).HasMaxLength(200);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AccountID)
                    .HasConstraintName("FK_Order_Account");
            });

			modelBuilder.Entity<OrderDetail>(entity =>
			{
				entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID").ValueGeneratedOnAdd();
				entity.Property(e => e.OrderId).HasColumnName("OrderID");
				entity.Property(e => e.ProductId).HasColumnName("ProductID");

				entity.HasOne(d => d.Order)
					.WithMany(p => p.OrderDetails)
					.HasForeignKey(d => d.OrderId)
					.HasConstraintName("FK_OrderDetail_Order");

				entity.HasOne(d => d.Product)
					.WithMany(p => p.OrderDetails)
					.HasForeignKey(d => d.ProductId)
					.HasConstraintName("FK_OrderDetail_Product");
			});

			modelBuilder.Entity<Cart>(entity =>
			{
				entity.Property(e => e.CartID).HasColumnName("CartID").ValueGeneratedOnAdd();
				entity.Property(e => e.CustomerId).HasColumnName("CustomerId");
			
			});

			modelBuilder.Entity<Product>(entity =>
			{
				entity.Property(e => e.ProductId).HasColumnName("ProductID").ValueGeneratedOnAdd();
				entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
				entity.Property(e => e.ProductName).HasMaxLength(100);
				//ThumbnailImage
				
				entity.Property(e => e.DateCreated).HasColumnType("datetime")
												.HasDefaultValueSql("GETDATE()"); 
				entity.HasOne(d => d.Category)
					.WithMany(p => p.Products)
					.HasForeignKey(d => d.CategoryId)
					.HasConstraintName("FK_Products_Category");
				entity.HasOne(d => d.Cart)
					.WithMany(p => p.Products)
					.HasForeignKey(d => d.CartId)
					.HasConstraintName("FK_Products_Cart");
			});

			modelBuilder.Entity<ProductImages>(entity =>
			{
				entity.HasKey(e => e.ProductImageId);
				entity.Property(e => e.ProductImageId).HasColumnName("ProductImageId").ValueGeneratedOnAdd();
				entity.Property(e => e.ProductId).HasColumnName("ProductID");
				entity.Property(e => e.ImagePath).HasColumnName("ImagePath");
				entity.Property(e => e.IsDefault).HasColumnName("IsDefault");
				entity.Property(e => e.Caption).HasColumnName("Caption");

				entity.HasOne(d => d.Product)
					.WithMany(p => p.ProductImages)
					.HasForeignKey(d => d.ProductId)
					.HasConstraintName("FK_ProductImages_Product");
			});

			modelBuilder.Entity<Rating>(entity =>
			{
				entity.Property(e => e.RatingId).HasColumnName("RatingID").ValueGeneratedOnAdd();
				entity.Property(e => e.AccountId).HasColumnName("AccountID"); // Phải trùng kiểu dữ liệu
				entity.Property(e => e.ProductId).HasColumnName("ProductID");
				entity.Property(e => e.RatingDate).HasColumnType("datetime");

				entity.HasOne(d => d.Account)
					.WithMany(p => p.Ratings)
					.HasForeignKey(d => d.AccountId)
					.HasConstraintName("FK_Rating_Account");

				entity.HasOne(d => d.Product)
					.WithMany(p => p.Ratings)
					.HasForeignKey(d => d.ProductId)
					.HasConstraintName("FK_Rating_Product");
			});
			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

	}

}
