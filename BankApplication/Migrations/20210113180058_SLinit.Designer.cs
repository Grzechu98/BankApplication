﻿// <auto-generated />
using System;
using BankApplication.SharedLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankApplication.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20210113180058_SLinit")]
    partial class SLinit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.AccountSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BankAccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("DailyOperationLimit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MaxDailyOperationsNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("SingleOperationLimit")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId")
                        .IsUnique();

                    b.ToTable("AccountSettings");
                });

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.AddressModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.BankAccountModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountNumber")
                        .IsUnique()
                        .HasFilter("[AccountNumber] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.OperationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("OperationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RecipientId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("IdentityDocumentExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdentityDocumentNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PIN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlaceOfBirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Secondname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("PIN", "IdentityDocumentNumber", "Email", "PhoneNumber", "Login")
                        .IsUnique()
                        .HasFilter("[PIN] IS NOT NULL AND [IdentityDocumentNumber] IS NOT NULL AND [Email] IS NOT NULL AND [PhoneNumber] IS NOT NULL AND [Login] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.AccountSettings", b =>
                {
                    b.HasOne("BankApplication.SharedLibrary.Models.BankAccountModel", "BankAccount")
                        .WithOne("Settings")
                        .HasForeignKey("BankApplication.SharedLibrary.Models.AccountSettings", "BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.BankAccountModel", b =>
                {
                    b.HasOne("BankApplication.SharedLibrary.Models.UserModel", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.OperationModel", b =>
                {
                    b.HasOne("BankApplication.SharedLibrary.Models.BankAccountModel", "Recipient")
                        .WithMany("Incomings")
                        .HasForeignKey("RecipientId");

                    b.HasOne("BankApplication.SharedLibrary.Models.BankAccountModel", "Sender")
                        .WithMany("Outgoings")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BankApplication.SharedLibrary.Models.UserModel", b =>
                {
                    b.HasOne("BankApplication.SharedLibrary.Models.AddressModel", "Address")
                        .WithMany("Residents")
                        .HasForeignKey("AddressId");
                });
#pragma warning restore 612, 618
        }
    }
}
