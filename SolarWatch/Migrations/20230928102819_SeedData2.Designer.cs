﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolarWatch;

#nullable disable

namespace SolarWatch.Migrations
{
    [DbContext(typeof(SolarWatchApiContext))]
    [Migration("20230928102819_SeedData2")]
    partial class SeedData2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SolarWatch.Model.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CityId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("SolarWatch.Model.SetRiseTime", b =>
                {
                    b.Property<int>("SetRiseTimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SetRiseTimeId"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Sunrise")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("Sunset")
                        .HasColumnType("time");

                    b.HasKey("SetRiseTimeId");

                    b.HasIndex("CityId")
                        .IsUnique();

                    b.ToTable("SetRiseTimes");
                });

            modelBuilder.Entity("SolarWatch.Model.SetRiseTime", b =>
                {
                    b.HasOne("SolarWatch.Model.City", "CityData")
                        .WithOne("SetRise")
                        .HasForeignKey("SolarWatch.Model.SetRiseTime", "CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CityData");
                });

            modelBuilder.Entity("SolarWatch.Model.City", b =>
                {
                    b.Navigation("SetRise")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
