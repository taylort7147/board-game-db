﻿// <auto-generated />
using BoardGameDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BoardGameDB.Migrations
{
    [DbContext(typeof(BoardGameDBContext))]
    [Migration("20220725194355_BoardGameGeekUrlToId")]
    partial class BoardGameGeekUrlToId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("NOCASE")
                .HasAnnotation("ProductVersion", "6.0.6");

            modelBuilder.Entity("BoardGameDB.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BoardGameDB.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BoardGameGeekId")
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int?>("Complexity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("MaximumPlayTimeMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaximumPlayerCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinimumPlayTimeMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinimumPlayerCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("PrimaryMechanicId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RulesUrl")
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("RulesVideoUrl")
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryMechanicId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("BoardGameDB.Models.Mechanic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.ToTable("Mechanic");
                });

            modelBuilder.Entity("BoardGameDB.Models.PlayStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.ToTable("PlayStyle");
                });

            modelBuilder.Entity("CategoryGame", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GamesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CategoriesId", "GamesId");

                    b.HasIndex("GamesId");

                    b.ToTable("CategoryGame");
                });

            modelBuilder.Entity("GameMechanic", b =>
                {
                    b.Property<int>("GamesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MechanicsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GamesId", "MechanicsId");

                    b.HasIndex("MechanicsId");

                    b.ToTable("GameMechanic");
                });

            modelBuilder.Entity("GamePlayStyle", b =>
                {
                    b.Property<int>("GamesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayStylesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GamesId", "PlayStylesId");

                    b.HasIndex("PlayStylesId");

                    b.ToTable("GamePlayStyle");
                });

            modelBuilder.Entity("BoardGameDB.Models.Game", b =>
                {
                    b.HasOne("BoardGameDB.Models.Mechanic", "PrimaryMechanic")
                        .WithMany()
                        .HasForeignKey("PrimaryMechanicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PrimaryMechanic");
                });

            modelBuilder.Entity("CategoryGame", b =>
                {
                    b.HasOne("BoardGameDB.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BoardGameDB.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameMechanic", b =>
                {
                    b.HasOne("BoardGameDB.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BoardGameDB.Models.Mechanic", null)
                        .WithMany()
                        .HasForeignKey("MechanicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GamePlayStyle", b =>
                {
                    b.HasOne("BoardGameDB.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BoardGameDB.Models.PlayStyle", null)
                        .WithMany()
                        .HasForeignKey("PlayStylesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
