using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class RevenueSummaryDto
    {
        public decimal TicketRevenue { get; set; }
        public decimal ComboRevenue { get; set; }
        public decimal TotalRevenue => TicketRevenue + ComboRevenue;
        public int SuccessfulTransactions { get; set; }
        public int TicketsSold { get; set; }
        public int KioskTransactions { get; set; }
        public int PosTransactions { get; set; }
    }

    public class MovieRevenueReportDto
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public int ShowtimeCount { get; set; }
        public int TicketsSold { get; set; }
        public int TotalSeats { get; set; }
        public int SeatsFilled { get; set; }
        public decimal TicketRevenue { get; set; }
        public decimal ComboRevenue { get; set; }
        public decimal TotalRevenue => TicketRevenue + ComboRevenue;
        public int SuccessfulTransactionsCount { get; set; }
        public double AverageOccupancy => TotalSeats > 0 ? (double)SeatsFilled / TotalSeats * 100 : 0;
    }

    public class ShowtimeOccupancyReportDto
    {
        public int ShowtimeId { get; set; }
        public int RoomId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
        public double OccupancyRate => TotalSeats > 0 ? (double)BookedSeats / TotalSeats * 100 : 0;
        public string PerformanceStatus => ReportService.ClassifyOccupancy(OccupancyRate);
    }

    public class ReportService
    {
        public const double HotThreshold = 80;
        public const double ColdThreshold = 30;

        public static string ClassifyOccupancy(double rate)
            => rate >= HotThreshold ? "🔥 Cháy vé" : rate < ColdThreshold ? "⚠️ Vắng" : "Ổn định";

        private static IQueryable<Booking> PaidBookings(CinemaDbContext db, DateTime from, DateTime to)
            => db.Bookings.AsNoTracking()
                .Where(b => b.CreatedAt >= from && b.CreatedAt < to
                            && b.Payments.Any(p => p.Status == PaymentStatus.Success));

        public RevenueSummaryDto GetSummary(DateTime from, DateTime to)
        {
            using var db = DbFactory.Create();
            var rows = PaidBookings(db, from, to)
                .Select(b => new { b.TicketAmount, b.ComboAmount, b.Channel, Tickets = b.Tickets.Count() })
                .ToList();

            return new RevenueSummaryDto
            {
                TicketRevenue = rows.Sum(r => r.TicketAmount),
                ComboRevenue = rows.Sum(r => r.ComboAmount),
                SuccessfulTransactions = rows.Count,
                TicketsSold = rows.Sum(r => r.Tickets),
                KioskTransactions = rows.Count(r => r.Channel == SalesChannels.Kiosk),
                PosTransactions = rows.Count(r => r.Channel == SalesChannels.Pos)
            };
        }

        public List<MovieRevenueReportDto> GetRevenueByMovie(DateTime from, DateTime to)
        {
            using var db = DbFactory.Create();

            var revenueRows = PaidBookings(db, from, to)
                .Select(b => new
                {
                    MovieId = b.ShowtimeId == null ? 0 : b.Showtime!.MovieId,
                    Title = b.ShowtimeId == null ? BookingService.ComboOnlyTitle : b.Showtime!.Movie.Title,
                    b.TicketAmount,
                    b.ComboAmount,
                    Tickets = b.Tickets.Count()
                })
                .ToList();

            var showtimeRows = db.Showtimes.AsNoTracking()
                .Where(s => s.StartTime >= from && s.StartTime < to)
                .Select(s => new
                {
                    s.MovieId,
                    s.Movie.Title,
                    Seats = s.Room.Seats.Count(),
                    Sold = s.Tickets.Count(t => t.Booking.Payments.Any(p => p.Status == PaymentStatus.Success))
                })
                .ToList();

            var movieIds = revenueRows.Select(r => r.MovieId).Union(showtimeRows.Select(s => s.MovieId)).Distinct();

            return movieIds.Select(id =>
                {
                    var rev = revenueRows.Where(r => r.MovieId == id).ToList();
                    var shows = showtimeRows.Where(s => s.MovieId == id).ToList();
                    return new MovieRevenueReportDto
                    {
                        MovieId = id,
                        MovieName = rev.FirstOrDefault()?.Title ?? shows.FirstOrDefault()?.Title ?? $"Phim #{id}",
                        ShowtimeCount = shows.Count,
                        TotalSeats = shows.Sum(s => s.Seats),
                        SeatsFilled = shows.Sum(s => s.Sold),
                        TicketsSold = rev.Sum(r => r.Tickets),
                        TicketRevenue = rev.Sum(r => r.TicketAmount),
                        ComboRevenue = rev.Sum(r => r.ComboAmount),
                        SuccessfulTransactionsCount = rev.Count
                    };
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ThenBy(r => r.MovieName)
                .ToList();
        }

        public List<ShowtimeOccupancyReportDto> GetShowtimeOccupancy(DateTime from, DateTime to, int? roomId = null)
        {
            using var db = DbFactory.Create();
            var query = db.Showtimes.AsNoTracking().Where(s => s.StartTime >= from && s.StartTime < to);
            if (roomId.HasValue) query = query.Where(s => s.RoomId == roomId.Value);

            return query
                .OrderBy(s => s.RoomId).ThenBy(s => s.StartTime)
                .Select(s => new ShowtimeOccupancyReportDto
                {
                    ShowtimeId = s.ShowtimeId,
                    RoomId = s.RoomId,
                    MovieName = s.Movie.Title,
                    RoomName = s.Room.RoomName,
                    StartTime = s.StartTime,
                    TotalSeats = s.Room.Seats.Count(),
                    BookedSeats = s.Tickets.Count(t => t.Booking.Payments.Any(p => p.Status == PaymentStatus.Success))
                })
                .ToList();
        }
    }
}
