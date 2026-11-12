using System;
using System.Collections.Generic;
using System.Linq;
using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class ShowtimeService
    {
        private const int CLEANING_BUFFER_MINUTES = 15; // Thời gian dọn dẹp phòng giữa 2 suất chiếu

        /// <summary>
        /// Kiểm tra suất chiếu mới có bị trùng thời gian với các suất chiếu đã có trong cùng phòng hay không.
        /// </summary>
        public (bool IsOverlapping, string Message) CheckShowtimeOverlap(int roomId, DateTime startTime, int movieDurationMinutes, int? ignoreShowtimeId = null)
        {
            DateTime endTimeWithBuffer = startTime.AddMinutes(movieDurationMinutes + CLEANING_BUFFER_MINUTES);

            using var db = new CinemaDbContext();
            
            var existingShowtimes = db.Showtimes
                .Include(s => s.Movie)
                .Where(s => s.RoomId == roomId)
                .ToList();

            if (ignoreShowtimeId.HasValue)
            {
                existingShowtimes = existingShowtimes.Where(s => s.ShowtimeId != ignoreShowtimeId.Value).ToList();
            }

            foreach (var st in existingShowtimes)
            {
                int existingDuration = st.Movie?.Duration ?? 120;
                DateTime existingEndWithBuffer = (st.EndTime ?? st.StartTime.AddMinutes(existingDuration)).AddMinutes(CLEANING_BUFFER_MINUTES);

                // Kiểm tra sự chồng lấp khoảng thời gian
                bool overlap = startTime < existingEndWithBuffer && endTimeWithBuffer > st.StartTime;

                if (overlap)
                {
                    string movieTitle = st.Movie?.Title ?? "Chưa rõ";
                    string msg = $"Trùng lịch chiếu! Phòng này đã có phim '{movieTitle}' từ {st.StartTime:HH:mm} đến {st.EndTime:HH:mm} (bao gồm 15 phút dọn phòng).";
                    return (true, msg);
                }
            }

            return (false, string.Empty);
        }

        /// <summary>
        /// Tạo mới suất chiếu
        /// </summary>
        public (bool Success, string Message) AddShowtime(int movieId, int roomId, DateTime startTime, decimal price)
        {
            using var db = new CinemaDbContext();

            var movie = db.Movies.Find(movieId);
            if (movie == null) return (false, "Không tìm thấy bộ phim được chọn.");

            var room = db.Rooms.Find(roomId);
            if (room == null) return (false, "Không tìm thấy phòng chiếu được chọn.");

            if (startTime <= DateTime.Now)
            {
                return (false, "Thời gian bắt đầu suất chiếu phải ở tương lai.");
            }

            var overlapCheck = CheckShowtimeOverlap(roomId, startTime, movie.Duration);
            if (overlapCheck.IsOverlapping)
            {
                return (false, overlapCheck.Message);
            }

            DateTime endTime = startTime.AddMinutes(movie.Duration);

            var showtime = new Showtime
            {
                MovieId = movieId,
                RoomId = roomId,
                StartTime = startTime,
                EndTime = endTime,
                Price = price
            };

            db.Showtimes.Add(showtime);
            db.SaveChanges();

            return (true, "Thêm suất chiếu thành công!");
        }

        /// <summary>
        /// Xóa suất chiếu
        /// </summary>
        public (bool Success, string Message) DeleteShowtime(int showtimeId)
        {
            using var db = new CinemaDbContext();

            var showtime = db.Showtimes
                .Include(s => s.Tickets)
                .FirstOrDefault(s => s.ShowtimeId == showtimeId);

            if (showtime == null) return (false, "Suất chiếu không tồn tại.");

            if (showtime.Tickets != null && showtime.Tickets.Any())
            {
                return (false, "Không thể xóa suất chiếu này vì đã có vé được bán ra!");
            }

            db.Showtimes.Remove(showtime);
            db.SaveChanges();

            return (true, "Đã xóa suất chiếu thành công.");
        }
    }
}