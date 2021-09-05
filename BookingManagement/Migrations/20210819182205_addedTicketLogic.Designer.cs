﻿// <auto-generated />
using System;
using BookingManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookingManagement.Migrations
{
    [DbContext(typeof(BookingManagementDbContext))]
    [Migration("20210819182205_addedTicketLogic")]
    partial class addedTicketLogic
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookingManagement.Models.Domain.Bus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BusPicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Buses");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Dates", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Dates");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Routes", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("BusId")
                        .HasColumnType("int");

                    b.Property<int>("CompandyId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<int>("EndStationId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("StartStationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusId");

                    b.HasIndex("EndStationId");

                    b.HasIndex("StartStationId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Seat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<bool>("Checked")
                        .HasColumnType("bit");

                    b.Property<string>("SeatCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusId");

                    b.ToTable("Seat");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TownId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TownId");

                    b.ToTable("Station");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Tickets", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("bit");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<string>("RouteId1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SeatId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RouteId1");

                    b.HasIndex("SeatId")
                        .IsUnique();

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Towns", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Country")
                        .HasColumnType("int");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Towns");
                });

            modelBuilder.Entity("DatesRoutes", b =>
                {
                    b.Property<int>("MyPropertyId")
                        .HasColumnType("int");

                    b.Property<string>("RoutesId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MyPropertyId", "RoutesId");

                    b.HasIndex("RoutesId");

                    b.ToTable("DatesRoutes");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Routes", b =>
                {
                    b.HasOne("BookingManagement.Models.Domain.Bus", "Bus")
                        .WithMany()
                        .HasForeignKey("BusId");

                    b.HasOne("BookingManagement.Models.Domain.Station", "EndStation")
                        .WithMany("EndStations")
                        .HasForeignKey("EndStationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BookingManagement.Models.Domain.Station", "StartStation")
                        .WithMany("StartStations")
                        .HasForeignKey("StartStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("EndStation");

                    b.Navigation("StartStation");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Seat", b =>
                {
                    b.HasOne("BookingManagement.Models.Domain.Bus", "Bus")
                        .WithMany("Seats")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Station", b =>
                {
                    b.HasOne("BookingManagement.Models.Domain.Towns", "Town")
                        .WithMany()
                        .HasForeignKey("TownId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Town");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Tickets", b =>
                {
                    b.HasOne("BookingManagement.Models.Domain.Routes", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId1");

                    b.HasOne("BookingManagement.Models.Domain.Seat", "Seat")
                        .WithOne("Ticket")
                        .HasForeignKey("BookingManagement.Models.Domain.Tickets", "SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Route");

                    b.Navigation("Seat");
                });

            modelBuilder.Entity("DatesRoutes", b =>
                {
                    b.HasOne("BookingManagement.Models.Domain.Dates", null)
                        .WithMany()
                        .HasForeignKey("MyPropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingManagement.Models.Domain.Routes", null)
                        .WithMany()
                        .HasForeignKey("RoutesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Bus", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Seat", b =>
                {
                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("BookingManagement.Models.Domain.Station", b =>
                {
                    b.Navigation("EndStations");

                    b.Navigation("StartStations");
                });
#pragma warning restore 612, 618
        }
    }
}
