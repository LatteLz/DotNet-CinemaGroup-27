using System;
using System.Collections.Generic;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Services;

public class BookingService
{
    private readonly CinemaDbContext _db;

    public BookingService()
    {
        _db = new CinemaDbContext();
    }
    public bool CreateBookingTransaction(int showtimeId, List<string> selectedSeats, decimal totalAmount, out string errorMessage)
    {
        errorMessage = string.Empty;
        using var transaction = _db.Database.BeginTransaction();

        try
        {
            var Showtime = _db.Showtimes.FirstOrDefault(s => s.ShowtimeId == showtimeId);
            string movieTitle = "N/A";
            string showtimeInfo = "N/A";

            if (Showtime != null)
            {
                var movie = _db.Movies.FirstOrDefault(m => m.MovieId == Showtime.MovieId);
                if (movie != null) movieTitle = movie.Title;
                showtimeInfo = $"{Showtime.StartTime:HH:mm dd/MM/yyyy} - Phòng {Showtime.RoomId}";
            }
            var booking = new Booking
            {
                MovieTitle = movieTitle,
                ShowtimeInfo = showtimeInfo,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.Now
            };

            _db.Bookings.Add(booking);
            _db.SaveChanges(); 
            
            foreach (var seatCode in selectedSeats)
            {
                var seatObj = _db.Seats.FirstOrDefault(s => s.SeatNumber == seatCode || s.SeatNumber == seatCode);
                int seatId = seatObj?.SeatId ?? 1; 
                var ticket = new Ticket
                {
                    BookingId = booking.Id, 
                    ShowtimeId = showtimeId,
                    SeatId = seatId,
                    Price = 160000m
                };

                _db.Tickets.Add(ticket);
            }

            _db.SaveChanges();
            transaction.Commit();
            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            errorMessage = ex.Message;
            return false;
        }
    }
}