﻿// <auto-generated />
using System;
using Mercure.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mercure.API.Migrations
{
    [DbContext(typeof(MercureContext))]
    [Migration("20230325211030_FixRoleTable")]
    partial class FixRoleTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.Property<int>("CategoriessCategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductsProductId")
                        .HasColumnType("integer");

                    b.HasKey("CategoriessCategoryId", "ProductsProductId");

                    b.HasIndex("ProductsProductId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("Mercure.API.Models.Animal", b =>
                {
                    b.Property<int>("AnimalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AnimalId"));

                    b.Property<DateTime>("AnimalBirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("AnimalColor")
                        .HasColumnType("text");

                    b.Property<DateTime>("AnimalCreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("AnimalLastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("AnimalPrice")
                        .HasColumnType("integer");

                    b.Property<int?>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("SpeciesId")
                        .HasColumnType("integer");

                    b.HasKey("AnimalId");

                    b.HasIndex("OrderId");

                    b.HasIndex("SpeciesId");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("Mercure.API.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryDescription")
                        .HasColumnType("text");

                    b.Property<string>("CategoryTitle")
                        .HasColumnType("text");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Mercure.API.Models.OAuth2Credentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessToken")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("OAuth2Credentials");
                });

            modelBuilder.Entity("Mercure.API.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderId"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OrderDeliveryPrice")
                        .HasColumnType("integer");

                    b.Property<int>("OrderPrice")
                        .HasColumnType("integer");

                    b.Property<bool>("OrderStatus")
                        .HasColumnType("boolean");

                    b.Property<int>("OrderTaxPrice")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Mercure.API.Models.OrderProduct", b =>
                {
                    b.Property<int>("OrderProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderProductId"));

                    b.Property<int?>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<bool>("Shipped")
                        .HasColumnType("boolean");

                    b.HasKey("OrderProductId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("Mercure.API.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<string>("ProductBrandName")
                        .HasColumnType("text");

                    b.Property<DateTime>("ProductCreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ProductDescription")
                        .HasColumnType("text");

                    b.Property<DateTime>("ProductLastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ProductName")
                        .HasColumnType("text");

                    b.Property<int>("ProductPrice")
                        .HasColumnType("integer");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId");

                    b.HasIndex("StockId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Mercure.API.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .HasColumnType("text");

                    b.Property<int>("RoleNumber")
                        .HasColumnType("integer");

                    b.HasKey("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Mercure.API.Models.Species", b =>
                {
                    b.Property<int>("SpeciesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SpeciesId"));

                    b.Property<string>("SpeciesName")
                        .HasColumnType("text");

                    b.HasKey("SpeciesId");

                    b.ToTable("Speciess");
                });

            modelBuilder.Entity("Mercure.API.Models.Stock", b =>
                {
                    b.Property<int>("StockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StockId"));

                    b.Property<int>("StockQuantityAvailable")
                        .HasColumnType("integer");

                    b.HasKey("StockId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("Mercure.API.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<string>("ServiceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.HasOne("Mercure.API.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriessCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mercure.API.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mercure.API.Models.Animal", b =>
                {
                    b.HasOne("Mercure.API.Models.Order", null)
                        .WithMany("Animals")
                        .HasForeignKey("OrderId");

                    b.HasOne("Mercure.API.Models.Species", "Species")
                        .WithMany()
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Species");
                });

            modelBuilder.Entity("Mercure.API.Models.OAuth2Credentials", b =>
                {
                    b.HasOne("Mercure.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mercure.API.Models.Order", b =>
                {
                    b.HasOne("Mercure.API.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mercure.API.Models.OrderProduct", b =>
                {
                    b.HasOne("Mercure.API.Models.Order", null)
                        .WithMany("Products")
                        .HasForeignKey("OrderId");

                    b.HasOne("Mercure.API.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Mercure.API.Models.Product", b =>
                {
                    b.HasOne("Mercure.API.Models.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Mercure.API.Models.User", b =>
                {
                    b.HasOne("Mercure.API.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Mercure.API.Models.Order", b =>
                {
                    b.Navigation("Animals");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Mercure.API.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
