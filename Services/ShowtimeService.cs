using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class ShowtimeListItemDto
    {
        public int ShowtimeId { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public int SoldSeats { get; set; }
        public int HeldSeats { get; set; }
        public int AvailableSeats => Math.Max(0, TotalSeats - SoldSeats - HeldSeats);
    }

    public class ShowtimeService
    {
        public const int CleaningBufferMinutes = 15;

        public const int SellUntilMinutesAfterStart = 15;

        public const int SalesWindowDays = 4;

        public static DateTime CalculateEndTime(DateTime startTime, int movieDurationMinutes)
            => startTime.AddMinutes(movieDurationMinutes + CleaningBufferMinutes);

        private static void EnsureNoOverlap(
            CinemaDbContext db, int roomId, DateTime newStart, DateTime newEnd, int? ignoreShowtimeId)
        {
            int ignoreId = ignoreShowtimeId ?? 0;
            var conflict = db.Showtimes
                .AsNoTracking()
                .Include(s => s.Movie)
                .Where(s => s.RoomId == roomId
                            && s.ShowtimeId != ignoreId
                            && s.StartTime < newEnd
                            && s.EndTime > newStart)
                .OrderBy(s => s.StartTime)
                .FirstOrDefault();

            if (conflict == null) return;

            throw new BusinessException($"Trùng lịch chiếu! Phòng này đã có phim '{conflict.Movie.Title}' " +
                         $"từ {conflict.StartTime:HH:mm} đến {conflict.EndTime:HH:mm} ngày {conflict.StartTime:dd/MM} " +
                         $"(đã gồm {CleaningBufferMinutes} phút dọn phòng).");
        }

        public void AddShowtime(int movieId, int roomId, DateTime startTime, decimal price)
        {
            using var db = DbFactory.Create();

            var movie = db.Movies.Find(movieId);
            if (movie == null) throw new BusinessException("Không tìm thấy bộ phim được chọn.");
            if (db.Rooms.Find(roomId) == null) throw new BusinessException("Không tìm thấy phòng chiếu được chọn.");
            if (!db.Seats.Any(s => s.RoomId == roomId)) throw new BusinessException("Phòng chiếu chưa có sơ đồ ghế.");
            if (price <= 0) throw new BusinessException("Giá vé phải lớn hơn 0.");
            if (startTime <= DateTime.Now) throw new BusinessException("Thời gian bắt đầu suất chiếu phải ở tương lai.");

            DateTime endTime = CalculateEndTime(startTime, movie.Duration);
            EnsureNoOverlap(db, roomId, startTime, endTime, null);

            db.Showtimes.Add(new Showtime
            {
                MovieId = movieId,
                RoomId = roomId,
                StartTime = startTime,
                EndTime = endTime,
                Price = price
            });
            db.SaveChanges();
        }

        public void UpdateShowtime(int showtimeId, int movieId, int roomId, DateTime startTime, decimal price)
        {
            using var db = DbFactory.Create();

            var showtime = db.Showtimes.Find(showtimeId);
            if (showtime == null) throw new BusinessException("Suất chiếu không tồn tại.");

            var movie = db.Movies.Find(movieId);
            if (movie == null) throw new BusinessException("Không tìm thấy bộ phim được chọn.");
            if (db.Rooms.Find(roomId) == null) throw new BusinessException("Không tìm thấy phòng chiếu được chọn.");
            if (price <= 0) throw new BusinessException("Giá vé phải lớn hơn 0.");

            bool scheduleChanged = showtime.MovieId != movieId
                                   || showtime.RoomId != roomId
                                   || showtime.StartTime != startTime;

            if (scheduleChanged)
            {
                if (db.Tickets.Any(t => t.ShowtimeId == showtimeId))
                    throw new BusinessException("Suất chiếu đã có vé bán ra nên chỉ được đổi giá vé, không được đổi phim, phòng hoặc giờ chiếu.");
                if (startTime <= DateTime.Now)
                    throw new BusinessException("Thời gian bắt đầu suất chiếu phải ở tương lai.");

                DateTime endTime = CalculateEndTime(startTime, movie.Duration);
                EnsureNoOverlap(db, roomId, startTime, endTime, showtimeId);

                showtime.MovieId = movieId;
                showtime.RoomId = roomId;
                showtime.StartTime = startTime;
                showtime.EndTime = endTime;
            }

            showtime.Price = price;
            db.SaveChanges();
        }

        public void DeleteShowtime(int showtimeId)
        {
            using var db = DbFactory.Create();

            var showtime = db.Showtimes.Find(showtimeId);
            if (showtime == null) throw new BusinessException("Suất chiếu không tồn tại.");

            if (db.Tickets.Any(t => t.ShowtimeId == showtimeId) || db.Bookings.Any(b => b.ShowtimeId == showtimeId))
                throw new BusinessException("Không thể xóa suất chiếu này vì đã có vé được bán ra!");

            db.SeatHolds.RemoveRange(db.SeatHolds.Where(h => h.ShowtimeId == showtimeId));
            db.Showtimes.Remove(showtime);
            db.SaveChanges();
        }

        public ShowtimeListItemDto? GetById(int showtimeId)
            => Query(s => s.ShowtimeId == showtimeId).FirstOrDefault();

        public List<ShowtimeListItemDto> GetAllForAdmin(bool includePast)
        {
            if (includePast)
                return Query(_ => true).OrderByDescending(s => s.StartTime).ToList();

            DateTime today = DateTime.Today;
            return Query(s => s.StartTime >= today).OrderBy(s => s.StartTime).ToList();
        }

        public List<ShowtimeListItemDto> GetShowtimesForMovie(int movieId, DateTime date, string format = "All")
        {
            DateTime from = date.Date;
            DateTime to = from.AddDays(1);
            var list = Query(s => s.MovieId == movieId && s.StartTime >= from && s.StartTime < to);

            if (format == "IMAX")
                list = list.Where(s => IsImax(s)).ToList();
            else if (format == "Standard")
                list = list.Where(s => !IsImax(s)).ToList();

            return list.OrderBy(s => s.StartTime).ToList();
        }

        public List<ShowtimeListItemDto> GetSellableShowtimes()
        {
            DateTime cutoff = DateTime.Now.AddMinutes(-SellUntilMinutesAfterStart);
            DateTime until = DateTime.Today.AddDays(SalesWindowDays);
            return Query(s => s.StartTime > cutoff && s.StartTime < until)
                .OrderBy(s => s.StartTime)
                .ToList();
        }

        public static bool IsSellable(DateTime startTime)
            => DateTime.Now <= startTime.AddMinutes(SellUntilMinutesAfterStart);

        private static bool IsImax(ShowtimeListItemDto s)
            => s.RoomType.Contains("IMAX", StringComparison.OrdinalIgnoreCase)
               || s.RoomName.Contains("IMAX", StringComparison.OrdinalIgnoreCase);

        private static List<ShowtimeListItemDto> Query(System.Linq.Expressions.Expression<Func<Showtime, bool>> predicate)
        {
            using var db = DbFactory.Create();
            DateTime now = DateTime.Now;

            return db.Showtimes
                .AsNoTracking()
                .Where(predicate)
                .Select(s => new ShowtimeListItemDto
                {
                    ShowtimeId = s.ShowtimeId,
                    MovieId = s.MovieId,
                    RoomId = s.RoomId,
                    MovieTitle = s.Movie.Title,
                    Duration = s.Movie.Duration,
                    RoomName = s.Room.RoomName,
                    RoomType = s.Room.RoomType,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Price = s.Price,
                    TotalSeats = s.Room.Seats.Count(),
                    SoldSeats = s.Tickets.Count(),
                    HeldSeats = s.SeatHolds.Count(h => h.ExpiresAt > now)
                })
                .ToList();
        }
    }
}
