﻿// <auto-generated />
using System;
using BidProjectsManager.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BidProjectsManager.DataLayer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230101105351_Initial-Database")]
    partial class InitialDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BidProjectsManager.Model.Entities.BidCapex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Value")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("BidCapexes");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.BidEbit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Value")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("BidEbits");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.BidOpex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Value")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("BidOpexes");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.DictionaryType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DictionaryTypes");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.DictionaryValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DictionaryTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryTypeId");

                    b.ToTable("DictionaryValues");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("BidEstiamtedOperationEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("BidOperationStart")
                        .HasColumnType("datetime2");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LifetimeInThousandsKilometers")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoBidReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumberOfVechicles")
                        .HasColumnType("int");

                    b.Property<int?>("OptionalExtensionYears")
                        .HasColumnType("int");

                    b.Property<int?>("Priority")
                        .HasColumnType("int");

                    b.Property<int?>("Probability")
                        .HasColumnType("int");

                    b.Property<int>("Stage")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalCapex")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal?>("TotalEbit")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal?>("TotalOpex")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.ProjectComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CountryUser", b =>
                {
                    b.Property<int>("CountriesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("CountriesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserCountries", (string)null);
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.BidCapex", b =>
                {
                    b.HasOne("BidProjectsManager.Model.Entities.Project", "Project")
                        .WithMany("Capexes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.BidEbit", b =>
                {
                    b.HasOne("BidProjectsManager.Model.Entities.Project", "Project")
                        .WithMany("Ebits")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.BidOpex", b =>
                {
                    b.HasOne("BidProjectsManager.Model.Entities.Project", "Project")
                        .WithMany("Opexes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.Country", b =>
                {
                    b.HasOne("BidProjectsManager.Model.Entities.Currency", "Currency")
                        .WithMany("Countries")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.DictionaryValue", b =>
                {
                    b.HasOne("BidProjectsManager.Model.Entities.DictionaryType", "Type")
                        .WithMany("Values")
                        .HasForeignKey("DictionaryTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.Project", b =>
                {
                    b.HasOne("BidProjectsManager.Model.Entities.Country", "Country")
                        .WithMany("Projects")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BidProjectsManager.Model.Entities.Currency", "ProjectCurrency")
                        .WithMany("Projects")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("ProjectCurrency");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.ProjectComment", b =>
                {
                    b.HasOne("BidProjectsManager.Model.Entities.Project", "Project")
                        .WithMany("Comments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("CountryUser", b =>
                {
                    b.HasOne("BidProjectsManager.Model.Entities.Country", null)
                        .WithMany()
                        .HasForeignKey("CountriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BidProjectsManager.Model.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.Country", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.Currency", b =>
                {
                    b.Navigation("Countries");

                    b.Navigation("Projects");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.DictionaryType", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("BidProjectsManager.Model.Entities.Project", b =>
                {
                    b.Navigation("Capexes");

                    b.Navigation("Comments");

                    b.Navigation("Ebits");

                    b.Navigation("Opexes");
                });
#pragma warning restore 612, 618
        }
    }
}
