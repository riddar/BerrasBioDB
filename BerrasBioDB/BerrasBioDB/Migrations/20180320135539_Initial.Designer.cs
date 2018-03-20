﻿// <auto-generated />
using BerrasBioDB.App_Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BerrasBioDB.Migrations
{
    [DbContext(typeof(BerrasBioDBContext))]
    [Migration("20180320135539_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BerrasBioDB.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BerrasBioDB.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("BerrasBioDB.Models.Seat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("BerrasBioDB.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CustomerId");

                    b.Property<int?>("MovieId");

                    b.Property<int?>("SeatId");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("MovieId");

                    b.HasIndex("SeatId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("BerrasBioDB.Models.Venue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MaxSeats");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("BerrasBioDB.Models.Seat", b =>
                {
                    b.HasOne("BerrasBioDB.Models.Venue", "Venue")
                        .WithMany("Seats")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("BerrasBioDB.Models.Ticket", b =>
                {
                    b.HasOne("BerrasBioDB.Models.Customer", "Customer")
                        .WithMany("Tickets")
                        .HasForeignKey("CustomerId");

                    b.HasOne("BerrasBioDB.Models.Movie", "Movie")
                        .WithMany("Tickets")
                        .HasForeignKey("MovieId");

                    b.HasOne("BerrasBioDB.Models.Seat", "Seat")
                        .WithMany("Tickets")
                        .HasForeignKey("SeatId");
                });
#pragma warning restore 612, 618
        }
    }
}
