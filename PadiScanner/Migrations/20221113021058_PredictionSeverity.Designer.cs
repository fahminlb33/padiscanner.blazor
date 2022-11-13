﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PadiScanner.Infra;

#nullable disable

namespace PadiScanner.Migrations
{
    [DbContext(typeof(PadiDataContext))]
    [Migration("20221113021058_PredictionSeverity")]
    partial class PredictionSeverity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PadiScanner.Data.PredictionHistory", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("ClippedImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HeatmapImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("OriginalImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OverlayedImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Probabilities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Result")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Severity")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UploaderId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("Id");

                    b.HasIndex("UploaderId");

                    b.ToTable("Predictions");
                });

            modelBuilder.Entity("PadiScanner.Data.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "01GE24HFHHQZRRN024W32W8XF7",
                            FullName = "Fahmi Noor Fiqri",
                            LastLoginAt = new DateTime(2022, 11, 13, 9, 10, 57, 483, DateTimeKind.Local).AddTicks(9429),
                            Password = "$2a$11$aGidx/nFict4j13pFKERFeyCT2I1ZU6jxRc35cFg2cFIWQmK0OPCq",
                            Role = 0,
                            Username = "fahmi"
                        },
                        new
                        {
                            Id = "01GEBQMKK8SA2H2RSFXSCJFTMT",
                            FullName = "Hanif Hanan Al-Jufri",
                            LastLoginAt = new DateTime(2022, 11, 13, 9, 10, 57, 630, DateTimeKind.Local).AddTicks(8613),
                            Password = "$2a$11$ac3yaZA0M/dE3EsWmvB0fOoqVCzAdX7K4z7qMT8Uk.n9qJp1RuFcu",
                            Role = 1,
                            Username = "hanif"
                        },
                        new
                        {
                            Id = "01GEBQQ94E0Z8JBWGVJQNNH1N6",
                            FullName = "Abimanyu Okysaputra Rachman",
                            LastLoginAt = new DateTime(2022, 11, 13, 9, 10, 57, 777, DateTimeKind.Local).AddTicks(9765),
                            Password = "$2a$11$s2nii6iejHTuaNzaGbNIDuNbAQuBaBQAbcwa/ef/cYS8QSHUAdT22",
                            Role = 1,
                            Username = "abimanyu"
                        },
                        new
                        {
                            Id = "01GE24MT8165ZNXACDZYC8GMEQ",
                            FullName = "Tamu",
                            LastLoginAt = new DateTime(2022, 11, 13, 9, 10, 57, 924, DateTimeKind.Local).AddTicks(5234),
                            Password = "$2a$11$Tr5GaRzgWDBZ17bpZrlZI.dIo08clfKumh077rFErYTvLJZ0x.2ky",
                            Role = 2,
                            Username = "tamu"
                        });
                });

            modelBuilder.Entity("PadiScanner.Data.PredictionHistory", b =>
                {
                    b.HasOne("PadiScanner.Data.User", "Uploader")
                        .WithMany("Predictions")
                        .HasForeignKey("UploaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Uploader");
                });

            modelBuilder.Entity("PadiScanner.Data.User", b =>
                {
                    b.Navigation("Predictions");
                });
#pragma warning restore 612, 618
        }
    }
}