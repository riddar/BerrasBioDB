using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BerrasBio.Models;

namespace BerrasBio.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Seat> Seats { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Ticket>().HasKey(t => t.Id);
            builder.Entity<Ticket>().HasOne(t => t.User).WithMany(u => u.Tickets);
            builder.Entity<Ticket>().HasOne(t => t.Movie).WithMany(u => u.Tickets);
            builder.Entity<Ticket>().HasOne(t => t.Seat).WithMany(u => u.Tickets);

            builder.Entity<ApplicationUser>().HasMany(a => a.Tickets).WithOne(t => t.User);

            builder.Entity<Movie>().HasKey(m => m.Id);
            builder.Entity<Movie>().HasMany(m => m.Tickets).WithOne(t => t.Movie);

            builder.Entity<Seat>().HasKey(s => s.Id);
            builder.Entity<Seat>().HasMany(s => s.Tickets).WithOne(t => t.Seat);
            builder.Entity<Seat>().HasOne(s => s.Venue).WithMany(v => v.Seats);

            builder.Entity<Venue>().HasKey(v => v.Id);
            builder.Entity<Venue>().HasMany(v => v.Seats).WithOne(s => s.Venue);
        }
    }
}
