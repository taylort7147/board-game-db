﻿// <auto-generated />
using System;
using BoardGameDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BoardGameDB.Migrations
{
    [DbContext(typeof(BoardGameDBContext))]
    partial class BoardGameDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.6");

            modelBuilder.Entity("BoardGameDB.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BoardGameGeekUrl")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Complexity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("TEXT");

                    b.Property<int>("MaximumPlayTimeMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaximumPlayerCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinimumPlayTimeMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinimumPlayerCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("RulesUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("RulesVideoUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("BoardGameDB.Models.GameGameType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("GameTypeId");

                    b.ToTable("GameGameType");
                });

            modelBuilder.Entity("BoardGameDB.Models.GameMechanic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MechanicId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("MechanicId");

                    b.ToTable("GameMechanic");
                });

            modelBuilder.Entity("BoardGameDB.Models.GamePlayStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayStyleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayStyleId");

                    b.ToTable("GamePlayStyle");
                });

            modelBuilder.Entity("BoardGameDB.Models.GameType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("GameType");
                });

            modelBuilder.Entity("BoardGameDB.Models.Mechanic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Mechanic");
                });

            modelBuilder.Entity("BoardGameDB.Models.PlayStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("PlayStyle");
                });

            modelBuilder.Entity("BoardGameDB.Models.GameGameType", b =>
                {
                    b.HasOne("BoardGameDB.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BoardGameDB.Models.GameType", "GameType")
                        .WithMany()
                        .HasForeignKey("GameTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("GameType");
                });

            modelBuilder.Entity("BoardGameDB.Models.GameMechanic", b =>
                {
                    b.HasOne("BoardGameDB.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BoardGameDB.Models.Mechanic", "Mechanic")
                        .WithMany()
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Mechanic");
                });

            modelBuilder.Entity("BoardGameDB.Models.GamePlayStyle", b =>
                {
                    b.HasOne("BoardGameDB.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BoardGameDB.Models.PlayStyle", "PlayStyle")
                        .WithMany()
                        .HasForeignKey("PlayStyleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("PlayStyle");
                });

            modelBuilder.Entity("BoardGameDB.Models.GameType", b =>
                {
                    b.HasOne("BoardGameDB.Models.Game", null)
                        .WithMany("GameTypes")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("BoardGameDB.Models.Mechanic", b =>
                {
                    b.HasOne("BoardGameDB.Models.Game", null)
                        .WithMany("Mechanics")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("BoardGameDB.Models.PlayStyle", b =>
                {
                    b.HasOne("BoardGameDB.Models.Game", null)
                        .WithMany("PlayStyles")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("BoardGameDB.Models.Game", b =>
                {
                    b.Navigation("GameTypes");

                    b.Navigation("Mechanics");

                    b.Navigation("PlayStyles");
                });
#pragma warning restore 612, 618
        }
    }
}
