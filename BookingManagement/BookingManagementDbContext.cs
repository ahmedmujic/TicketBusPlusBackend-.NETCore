using BookingManagement.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement
{
    public class BookingManagementDbContext : DbContext
    {
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Dates> Dates { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<Station> Station { get; set; }
        public DbSet<Towns> Towns { get; set; }

        public BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>().HasMany(s => s.StartStations)
                .WithOne(r => r.StartStation)
                .HasForeignKey(r => r.StartStationId);
            modelBuilder.Entity<Station>().HasMany(s => s.EndStations)
                .WithOne(r => r.EndStation)
                .HasForeignKey(r => r.EndStationId).OnDelete(DeleteBehavior.Restrict);
        }

        internal object AsNoTracking()
        {
            throw new NotImplementedException();
        }
    }
}
