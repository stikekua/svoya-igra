﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SvoyaIgra.DbMigrations.DbContexts;

#nullable disable

namespace SvoyaIgra.DbMigrations.Migrations.SvoyaIgra
{
    [DbContext(typeof(SvoyaIgraDbMigrationContext))]
    [Migration("20220615182447_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<string>("MultimediaId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nchar(36)")
                        .IsFixedLength();

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Question", "SvoyaIgra");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Answer = "Answer!",
                            Difficulty = 1,
                            MultimediaId = "00000000-0000-0000-0000-000000000000",
                            Text = "Question?",
                            ThemeId = 1,
                            Type = 1
                        });
                });

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Theme", "SvoyaIgra");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Difficulty = 1,
                            Name = "Tema1"
                        },
                        new
                        {
                            Id = 2,
                            Difficulty = 2,
                            Name = "Tema2"
                        });
                });

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Question", b =>
                {
                    b.HasOne("SvoyaIgra.Dal.Bo.Theme", "Theme")
                        .WithMany("Questions")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Theme", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}