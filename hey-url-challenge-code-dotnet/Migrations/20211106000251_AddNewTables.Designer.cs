﻿// <auto-generated />
using System;
using HeyUrlChallengeCodeDotnet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace hey_url_challenge_code_dotnet.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20211106000251_AddNewTables")]
    partial class AddNewTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("hey_url_challenge_code_dotnet.Models.Url", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("OriginalUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Urls");
                });

            modelBuilder.Entity("hey_url_challenge_code_dotnet.Models.VisitLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Browse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ClickDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("IdUrl")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Plataform")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdUrl");

                    b.ToTable("VisitLogs");
                });

            modelBuilder.Entity("hey_url_challenge_code_dotnet.Models.VisitLog", b =>
                {
                    b.HasOne("hey_url_challenge_code_dotnet.Models.Url", "Url")
                        .WithMany("VisitLogs")
                        .HasForeignKey("IdUrl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Url");
                });

            modelBuilder.Entity("hey_url_challenge_code_dotnet.Models.Url", b =>
                {
                    b.Navigation("VisitLogs");
                });
#pragma warning restore 612, 618
        }
    }
}