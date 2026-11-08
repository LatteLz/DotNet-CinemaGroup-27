using Microsoft.EntityFrameworkCore;
using CinemaManagement.Models;

namespace CinemaManagement.Data
{
    public class CinemaDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<Showtime> Showtimes { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<SeatHold> SeatHolds { get; set; } = null!;
        public DbSet<ComboItem> ComboItems { get; set; } = null!;
        public DbSet<BookingCombo> BookingCombos { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=cinema.db;Cache=Shared;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.ShowtimeId, t.SeatId })
                .IsUnique();

            modelBuilder.Entity<BookingCombo>()
                .HasKey(bc => bc.BookingComboId);

            modelBuilder.Entity<BookingCombo>()
                .HasOne(bc => bc.Booking)
                .WithMany()
                .HasForeignKey(bc => bc.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingCombo>()
                .HasOne(bc => bc.ComboItem)
                .WithMany(c => c.BookingCombos)
                .HasForeignKey(bc => bc.ComboId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}