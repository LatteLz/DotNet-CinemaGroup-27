using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;

namespace CinemaManagement.Services
{
    public class RevenueReportDto
    {
        public string MovieTitle { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class OccupancyReportDto
    {
        public int ShowtimeId { get; set; }
        public string MovieTitle { get; set; }
        public string ShowtimeInfo { get; set; }
        public int TotalSeats { get; set; }
        public int SoldSeats { get; set; }
        public double OccupancyRate { get; set; } 
        public string StatusLabel { get => OccupancyRate >= 80 ? "Phim cháy vé" : (OccupancyRate < 30 ? "Phim vắng" : "Bình thường"); }
    }

    public class ReportService
    {
        public List<RevenueReportDto> GetRevenueByMovie()
        {
            using var context = new CinemaDbContext();

            // Giả định Booking có liên kết với Movie và Payment thành công
            var report = context.Bookings
                .Where(b => b.Payments.Any(p => p.Status == "Success"))
                .GroupBy(b => b.MovieTitle)
                .Select(g => new RevenueReportDto
                {
                    MovieTitle = g.Key,
                    TotalTicketsSold = g.Count(),
                    TotalRevenue = g.Sum(b => b.TotalAmount)
                })
                .ToList();

            return report;
        }

        /// <summary>
        /// Thống kê tỷ lệ lấp đầy theo suất chiếu
        /// Công thức: Số vé đã bán ÷ tổng số ghế của phòng × 100%
        /// </summary>
        public List<OccupancyReportDto> GetOccupancyRateReport()
        {
            using var context = new CinemaDbContext();
            var showtimes = context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .ThenInclude(r => r.Seats)
                .ToList();

            var result = new List<OccupancyReportDto>();

            foreach (var st in showtimes)
            {
                int totalSeats = st.Room?.Seats?.Count ?? 0;

                // Đếm số vé đã bán dựa theo ShowtimeId (mỗi vé trong bảng Ticket ứng với 1 ghế đã bán)
                int soldSeats = context.Tickets.Count(t => t.ShowtimeId == st.ShowtimeId);

                double rate = totalSeats > 0 ? Math.Round((double)soldSeats / totalSeats * 100, 2) : 0;

                result.Add(new OccupancyReportDto
                {
                    ShowtimeId = st.ShowtimeId, // Sử dụng ShowtimeId khớp với Model
                    MovieTitle = st.Movie?.Title ?? "N/A",
                    ShowtimeInfo = st.StartTime.ToString("dd/MM/yyyy HH:mm"), // Sửa lại cú pháp ToString chuẩn
                    TotalSeats = totalSeats,
                    SoldSeats = soldSeats,
                    OccupancyRate = rate
                });
            }

            return result;
        }
    }
}