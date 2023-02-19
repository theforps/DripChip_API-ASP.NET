﻿// <auto-generated />
using System;
using DripChip_API.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DripChipAPI.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230219121941_updateLocationInfo")]
    partial class updateLocationInfo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("AnimalLocationInfo", b =>
                {
                    b.Property<long>("Animalsid")
                        .HasColumnType("INTEGER");

                    b.Property<long>("visitedLocationsid")
                        .HasColumnType("INTEGER");

                    b.HasKey("Animalsid", "visitedLocationsid");

                    b.HasIndex("visitedLocationsid");

                    b.ToTable("AnimalLocationInfo");
                });

            modelBuilder.Entity("AnimalTypes", b =>
                {
                    b.Property<long>("Animalsid")
                        .HasColumnType("INTEGER");

                    b.Property<long>("animalTypesid")
                        .HasColumnType("INTEGER");

                    b.HasKey("Animalsid", "animalTypesid");

                    b.HasIndex("animalTypesid");

                    b.ToTable("AnimalTypes");
                });

            modelBuilder.Entity("DripChip_API.Domain.Models.Animal", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("chipperId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("chippingDateTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("chippingLocationId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("deathDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("height")
                        .HasColumnType("REAL");

                    b.Property<float>("length")
                        .HasColumnType("REAL");

                    b.Property<string>("lifeStatus")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("weight")
                        .HasColumnType("REAL");

                    b.HasKey("id");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("DripChip_API.Domain.Models.Location", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("longitude")
                        .HasColumnType("REAL");

                    b.HasKey("id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("DripChip_API.Domain.Models.LocationInfo", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("dateTimeOfVisitLocationPoint")
                        .HasColumnType("TEXT");

                    b.Property<long>("locationPointid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("locationPointid");

                    b.ToTable("LocationInfo");
                });

            modelBuilder.Entity("DripChip_API.Domain.Models.Types", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("DripChip_API.Domain.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AnimalLocationInfo", b =>
                {
                    b.HasOne("DripChip_API.Domain.Models.Animal", null)
                        .WithMany()
                        .HasForeignKey("Animalsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DripChip_API.Domain.Models.LocationInfo", null)
                        .WithMany()
                        .HasForeignKey("visitedLocationsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AnimalTypes", b =>
                {
                    b.HasOne("DripChip_API.Domain.Models.Animal", null)
                        .WithMany()
                        .HasForeignKey("Animalsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DripChip_API.Domain.Models.Types", null)
                        .WithMany()
                        .HasForeignKey("animalTypesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DripChip_API.Domain.Models.LocationInfo", b =>
                {
                    b.HasOne("DripChip_API.Domain.Models.Location", "locationPoint")
                        .WithMany()
                        .HasForeignKey("locationPointid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("locationPoint");
                });
#pragma warning restore 612, 618
        }
    }
}
