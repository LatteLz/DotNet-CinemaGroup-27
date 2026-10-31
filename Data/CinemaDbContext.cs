using Microsoft.EntityFrameworkCore;
using CinemaManagement.Models;

namespace CinemaManagement.Data
{
    public class CinemaDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<SeatHold> SeatHolds { get; set; }
        public DbSet<ComboItem> ComboItems { get; set; }
        public DbSet<BookingCombo> BookingCombos { get; set; }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {optionsBuilder.UseSqlite("Data Source=cinema.db");
        }
    }
}