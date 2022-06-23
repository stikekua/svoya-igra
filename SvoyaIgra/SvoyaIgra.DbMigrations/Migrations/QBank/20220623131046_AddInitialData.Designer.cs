﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SvoyaIgra.DbMigrations.DbContexts;

#nullable disable

namespace SvoyaIgra.DbMigrations.Migrations.QBank
{
    [DbContext(typeof(QBankDbMigrationContext))]
    [Migration("20220623131046_AddInitialData")]
    partial class AddInitialData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Author", "QBank");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Test"
                        });
                });

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

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Question", "QBank");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Answer = "Answer1!",
                            Difficulty = 1,
                            MultimediaId = "00000000-0000-0000-0000-000000000000",
                            Text = "Question1?",
                            TopicId = 1,
                            Type = 1
                        },
                        new
                        {
                            Id = 2,
                            Answer = "Answer2!",
                            Difficulty = 2,
                            MultimediaId = "00000000-0000-0000-0000-000000000000",
                            Text = "Question2?",
                            TopicId = 1,
                            Type = 1
                        },
                        new
                        {
                            Id = 3,
                            Answer = "Answer!",
                            Difficulty = 3,
                            MultimediaId = "00000000-0000-0000-0000-000000000000",
                            Text = "Question3?",
                            TopicId = 1,
                            Type = 1
                        },
                        new
                        {
                            Id = 4,
                            Answer = "Answer4!",
                            Difficulty = 4,
                            MultimediaId = "00000000-0000-0000-0000-000000000000",
                            Text = "Question4?",
                            TopicId = 1,
                            Type = 1
                        },
                        new
                        {
                            Id = 5,
                            Answer = "Answer5!",
                            Difficulty = 5,
                            MultimediaId = "00000000-0000-0000-0000-000000000000",
                            Text = "Question5?",
                            TopicId = 1,
                            Type = 1
                        });
                });

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Topic", "QBank");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            Difficulty = 1,
                            Name = "Tema1"
                        });
                });

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Question", b =>
                {
                    b.HasOne("SvoyaIgra.Dal.Bo.Topic", "Topic")
                        .WithMany("Questions")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Topic", b =>
                {
                    b.HasOne("SvoyaIgra.Dal.Bo.Author", "Author")
                        .WithMany("Topics")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Author", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.Topic", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
