﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TOKENAPI.Models;

#nullable disable

namespace BackEndDevelopment.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BackEndDevelopment.Models.DTOS_FOR_RELATIONSHIPS.ProductCategoryImage", b =>
                {
                    b.Property<int?>("ProductCategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("ProductCategoryId", "ImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("ProductCategoriesImages", (string)null);
                });

            modelBuilder.Entity("BackEndDevelopment.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.OrderProps.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.OrderProps.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("TechnicalFrameId")
                        .HasColumnType("int");

                    b.Property<int?>("TechnicalSizeId")
                        .HasColumnType("int");

                    b.Property<int?>("TechnicalTypeId")
                        .HasColumnType("int");

                    b.Property<decimal?>("UnitPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("OrderId");

                    b.HasIndex("TechnicalFrameId");

                    b.HasIndex("TechnicalSizeId");

                    b.HasIndex("TechnicalTypeId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.PaperProperties.PaperFrame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("PaperFrames");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.PaperProperties.PaperSize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Acreage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("PaperSizes");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.PaperProperties.PaperType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("PaperTypes");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.ProductProps.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PImgUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.ProductProps.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TOKENAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.DTOS_FOR_RELATIONSHIPS.ProductCategoryImage", b =>
                {
                    b.HasOne("BackEndDevelopment.Models.Image", "Image")
                        .WithMany("ProductCategoryImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEndDevelopment.Models.ProductProps.ProductCategory", "ProductCategory")
                        .WithMany("ProductCategoryImages")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.Image", b =>
                {
                    b.HasOne("TOKENAPI.Models.User", "User")
                        .WithMany("Image")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.OrderProps.Order", b =>
                {
                    b.HasOne("TOKENAPI.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.OrderProps.OrderItem", b =>
                {
                    b.HasOne("BackEndDevelopment.Models.ProductProps.ProductCategory", "Category")
                        .WithMany("OrderItems")
                        .HasForeignKey("CategoryId");

                    b.HasOne("BackEndDevelopment.Models.OrderProps.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("BackEndDevelopment.Models.PaperProperties.PaperFrame", "TechnicalFrame")
                        .WithMany("OrderItems")
                        .HasForeignKey("TechnicalFrameId");

                    b.HasOne("BackEndDevelopment.Models.PaperProperties.PaperSize", "TechnicalSize")
                        .WithMany("OrderItems")
                        .HasForeignKey("TechnicalSizeId");

                    b.HasOne("BackEndDevelopment.Models.PaperProperties.PaperType", "TechnicalType")
                        .WithMany("OrderItems")
                        .HasForeignKey("TechnicalTypeId");

                    b.Navigation("Category");

                    b.Navigation("Order");

                    b.Navigation("TechnicalFrame");

                    b.Navigation("TechnicalSize");

                    b.Navigation("TechnicalType");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.ProductProps.ProductCategory", b =>
                {
                    b.HasOne("BackEndDevelopment.Models.ProductProps.Product", "Product")
                        .WithMany("Category")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.Image", b =>
                {
                    b.Navigation("ProductCategoryImages");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.OrderProps.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.PaperProperties.PaperFrame", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.PaperProperties.PaperSize", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.PaperProperties.PaperType", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.ProductProps.Product", b =>
                {
                    b.Navigation("Category");
                });

            modelBuilder.Entity("BackEndDevelopment.Models.ProductProps.ProductCategory", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("ProductCategoryImages");
                });

            modelBuilder.Entity("TOKENAPI.Models.User", b =>
                {
                    b.Navigation("Image");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
