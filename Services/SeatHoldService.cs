using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class SeatStatusDto
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
        public string SeatType { get; set; } = SeatTypes.Standard;
        public decimal Price { get; set; }
        public SeatState State { get; set; }

        public int ColumnSpan => SeatType == SeatTypes.Couple ? 2 : 1;
    }

    public class SeatMapDto
    {
        public int ShowtimeId { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public decimal BasePrice { get; set; }
        public List<SeatStatusDto> Seats { get; set; } = new();
    }

    public class SeatHoldService
    {
        public static readonly TimeSpan HoldDuration = TimeSpan.FromMinutes(3);

        public const int MaxSeatsPerOrder = 10;

        public static string NewSessionId() => Guid.NewGuid().ToString("N");

        public SeatMapDto GetSeatMap(int showtimeId, string sessionId)
        {
            using var db = DbFactory.Create();
            DateTime now = DateTime.Now;

            var showtime = db.Showtimes.AsNoTracking()
                               .Include(s => s.Room)
                               .FirstOrDefault(s => s.ShowtimeId == showtimeId)
                           ?? throw new BusinessException("Suất chiếu không tồn tại.");

            var seats = db.Seats.AsNoTracking()
                .Where(s => s.RoomId == showtime.RoomId)
                .OrderBy(s => s.RowIndex).ThenBy(s => s.ColIndex)
                .ToList();

            var soldIds = db.Tickets.Where(t => t.ShowtimeId == showtimeId)
                .Select(t => t.SeatId).ToHashSet();

            var holds = db.SeatHolds.AsNoTracking()
                .Where(h => h.ShowtimeId == showtimeId && h.ExpiresAt > now)
                .Select(h => new { h.SeatId, h.SessionId })
                .ToList();
            var myHolds = holds.Where(h => h.SessionId == sessionId).Select(h => h.SeatId).ToHashSet();
            var otherHolds = holds.Where(h => h.SessionId != sessionId).Select(h => h.SeatId).ToHashSet();

            return new SeatMapDto
            {
                ShowtimeId = showtimeId,
                RowCount = showtime.Room.RowCount,
                ColCount = showtime.Room.ColCount,
                BasePrice = showtime.Price,
                Seats = seats.Select(s => new SeatStatusDto
                {
                    SeatId = s.SeatId,
                    SeatNumber = s.SeatNumber,
                    RowIndex = s.RowIndex,
                    ColIndex = s.ColIndex,
                    SeatType = s.SeatType,
                    Price = SeatTypes.GetTicketPrice(showtime.Price, s.SeatType),
                    State = soldIds.Contains(s.SeatId) ? SeatState.Sold
                          : otherHolds.Contains(s.SeatId) ? SeatState.HeldByOther
                          : myHolds.Contains(s.SeatId) ? SeatState.HeldByMe
                          : SeatState.Available
                }).ToList()
            };
        }

        public bool TryHoldSeat(int showtimeId, int seatId, string sessionId, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentException("Thiếu mã phiên giữ ghế.", nameof(sessionId));

            using var db = DbFactory.Create();
            using var tx = db.Database.BeginTransaction();
            DateTime now = DateTime.Now;

            var showtime = db.Showtimes.Find(showtimeId);
            if (showtime == null) { message = "Suất chiếu không tồn tại."; return false; }
            if (!ShowtimeService.IsSellable(showtime.StartTime)) { message = "Suất chiếu đã quá giờ bán vé."; return false; }

            var seat = db.Seats.Find(seatId);
            if (seat == null || seat.RoomId != showtime.RoomId) { message = "Ghế không thuộc phòng của suất chiếu này."; return false; }

            if (db.Tickets.Any(t => t.ShowtimeId == showtimeId && t.SeatId == seatId))
            {
                message = $"Ghế {seat.SeatNumber} đã được bán.";
                return false;
            }

            var existing = db.SeatHolds.FirstOrDefault(h => h.ShowtimeId == showtimeId && h.SeatId == seatId);
            if (existing != null)
            {
                if (existing.ExpiresAt > now)
                {
                    if (existing.SessionId == sessionId) return true;
                    message = $"Ghế {seat.SeatNumber} đang được khách khác giữ. Vui lòng chọn ghế khác.";
                    return false;
                }
                db.SeatHolds.Remove(existing);
                db.SaveChanges();
            }

            int heldBySession = db.SeatHolds.Count(h => h.SessionId == sessionId && h.ExpiresAt > now);
            if (heldBySession >= MaxSeatsPerOrder)
            {
                message = $"Mỗi đơn chỉ được chọn tối đa {MaxSeatsPerOrder} ghế.";
                return false;
            }

            DateTime? sessionExpiry = db.SeatHolds
                .Where(h => h.SessionId == sessionId && h.ExpiresAt > now)
                .Select(h => (DateTime?)h.ExpiresAt)
                .Min();

            db.SeatHolds.Add(new SeatHold
            {
                ShowtimeId = showtimeId,
                SeatId = seatId,
                SessionId = sessionId,
                ExpiresAt = sessionExpiry ?? now.Add(HoldDuration)
            });

            try
            {
                db.SaveChanges();
                tx.Commit();
                return true;
            }
            catch (DbUpdateException)
            {
                message = $"Ghế {seat.SeatNumber} vừa được khách khác chọn. Vui lòng chọn ghế khác.";
                return false;
            }
        }

        public void ReleaseSeat(int showtimeId, int seatId, string sessionId)
        {
            using var db = DbFactory.Create();
            var holds = db.SeatHolds
                .Where(h => h.ShowtimeId == showtimeId && h.SeatId == seatId && h.SessionId == sessionId)
                .ToList();
            if (holds.Count == 0) return;
            db.SeatHolds.RemoveRange(holds);
            db.SaveChanges();
        }

        public void ReleaseSession(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) return;
            using var db = DbFactory.Create();
            var holds = db.SeatHolds.Where(h => h.SessionId == sessionId).ToList();
            if (holds.Count == 0) return;
            db.SeatHolds.RemoveRange(holds);
            db.SaveChanges();
        }

        public DateTime? GetSessionExpiry(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) return null;
            using var db = DbFactory.Create();
            DateTime now = DateTime.Now;
            return db.SeatHolds
                .Where(h => h.SessionId == sessionId && h.ExpiresAt > now)
                .Select(h => (DateTime?)h.ExpiresAt)
                .Min();
        }

        public void ReleaseExpiredHolds()
        {
            using var db = DbFactory.Create();
            DateTime now = DateTime.Now;
            var expired = db.SeatHolds.Where(h => h.ExpiresAt <= now).ToList();
            if (expired.Count == 0) return;
            db.SeatHolds.RemoveRange(expired);
            db.SaveChanges();
        }
    }
}
