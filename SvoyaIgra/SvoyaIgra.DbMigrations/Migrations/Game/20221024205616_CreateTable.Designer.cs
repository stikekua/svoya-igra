﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SvoyaIgra.DbMigrations.DbContexts;

#nullable disable

namespace SvoyaIgra.DbMigrations.Migrations.Game
{
    [DbContext(typeof(GameDbMigrationContext))]
    [Migration("20221024205616_CreateTable")]
    partial class CreateTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SvoyaIgra.Dal.Bo.GameSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ParametersConfig")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopicsConfig")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GameSessions", "Game");
                });
#pragma warning restore 612, 618
        }
    }
}
