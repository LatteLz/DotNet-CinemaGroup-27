using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaManagement.Services
{
    public class MovieRevenueReportDto
    {
        public string MovieName { get; set; }
        public decimal TicketRevenue { get; set; }
        public decimal ComboRevenue { get; set; }
        public decimal TotalRevenue => TicketRevenue + ComboRevenue;
        public int SuccessfulTransactionsCount { get; set; }
    }

    public class ShowtimeOccupancyReportDto
    {
        public string ShowtimeId { get; set; }
        public string MovieName { get; set; }
        public string RoomName { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
        public double OccupancyRate => TotalSeats > 0 ? ((double)BookedSeats / TotalSeats) * 100 : 0;
        public string PerformanceStatus => OccupancyRate >= 85 ? "🔥 Phim cháy vé" : (OccupancyRate < 30 ? "⚠️ Phim vắng" : "Ổn định");
    }

    public class TransactionModel
    {
        public string TransactionId { get; set; }
        public string MovieName { get; set; }
        public string Status { get; set; }
        public decimal TicketAmount { get; set; }
        public decimal ComboAmount { get; set; }
    }

    public class ShowtimeModel
    {
        public string ShowtimeId { get; set; }
        public string MovieName { get; set; }
        public string RoomName { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
    }

    public class ReportService
    {
        private readonly List<TransactionModel> _transactions;
        private readonly List<ShowtimeModel> _showtimes;

        public ReportService(List<TransactionModel> transactions, List<ShowtimeModel> showtimes)
        {
            _transactions = transactions ?? new List<TransactionModel>();
            _showtimes = showtimes ?? new List<ShowtimeModel>();
        }

        public List<MovieRevenueReportDto> GetRevenueByMovie()
        {
            return _transactions
                .Where(t => t.Status == "SUCCESS")
                .GroupBy(t => t.MovieName)
                .Select(g => new MovieRevenueReportDto
                {
                    MovieName = g.Key,
                    TicketRevenue = g.Sum(t => t.TicketAmount),
                    ComboRevenue = g.Sum(t => t.ComboAmount),
                    SuccessfulTransactionsCount = g.Count()
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToList();
        }

        public List<ShowtimeOccupancyReportDto> GetShowtimeOccupancyAndStatus()
        {
            return _showtimes
                .Select(s => new ShowtimeOccupancyReportDto
                {
                    ShowtimeId = s.ShowtimeId,
                    MovieName = s.MovieName,
                    RoomName = s.RoomName,
                    TotalSeats = s.TotalSeats,
                    BookedSeats = s.BookedSeats
                })
                .OrderByDescending(o => o.OccupancyRate)
                .ToList();
        }

        public List<ShowtimeOccupancyReportDto> GetHotOrColdMovies()
        {
            return GetShowtimeOccupancyAndStatus()
                .Where(o => o.OccupancyRate >= 85 || o.OccupancyRate < 30)
                .ToList();
        }
    }
}