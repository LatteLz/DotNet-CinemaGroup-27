using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class RoomSummaryDto
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public int StandardSeats { get; set; }
        public int VipSeats { get; set; }
        public int CoupleSeats { get; set; }
        public int TotalSeats { get; set; }
    }

    public class RoomService
    {
        public const int MaxRows = 26;
        public const int MaxCols = 20;

        public List<Room> GetAll()
        {
            using var db = DbFactory.Create();
            return db.Rooms.AsNoTracking().OrderBy(r => r.RoomName).ToList();
        }

        public List<RoomSummaryDto> GetSummaries()
        {
            using var db = DbFactory.Create();
            return db.Rooms.AsNoTracking()
                .OrderBy(r => r.RoomId)
                .Select(r => new RoomSummaryDto
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    RoomType = r.RoomType,
                    RowCount = r.RowCount,
                    ColCount = r.ColCount,
                    StandardSeats = r.Seats.Count(s => s.SeatType == SeatTypes.Standard),
                    VipSeats = r.Seats.Count(s => s.SeatType == SeatTypes.Vip),
                    CoupleSeats = r.Seats.Count(s => s.SeatType == SeatTypes.Couple),
                    TotalSeats = r.Seats.Count()
                })
                .ToList();
        }

        public Room CreateRoom(string roomName, string roomType, int rows, int cols, int vipRows, bool lastRowCouple)
        {
            roomName = (roomName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(roomName))
                throw new BusinessException("Tên phòng chiếu không được để trống.");
            if (rows < 1 || rows > MaxRows)
                throw new BusinessException($"Số hàng ghế phải từ 1 đến {MaxRows}.");
            if (cols < 1 || cols > MaxCols)
                throw new BusinessException($"Số ghế mỗi hàng phải từ 1 đến {MaxCols}.");
            if (vipRows < 0 || vipRows > rows)
                throw new BusinessException("Số hàng VIP không hợp lệ.");
            if (lastRowCouple && cols < 2)
                throw new BusinessException("Hàng ghế đôi cần ít nhất 2 ghế mỗi hàng.");

            using var db = DbFactory.Create();
            string lowerName = roomName.ToLower();
            if (db.Rooms.Any(r => r.RoomName.ToLower() == lowerName))
                throw new BusinessException($"Đã tồn tại phòng chiếu tên '{roomName}'.");

            using var tx = db.Database.BeginTransaction();
            var room = new Room { RoomName = roomName, RoomType = roomType, RowCount = rows, ColCount = cols };
            db.Rooms.Add(room);
            db.SaveChanges();

            db.Seats.AddRange(BuildSeats(room.RoomId, rows, cols, vipRows, lastRowCouple));
            db.SaveChanges();
            tx.Commit();
            return room;
        }

        public void DeleteRoom(int roomId)
        {
            using var db = DbFactory.Create();
            if (db.Showtimes.Any(s => s.RoomId == roomId))
                throw new BusinessException("Phòng này đã có lịch chiếu nên không thể xóa.");

            var room = db.Rooms.Find(roomId) ?? throw new BusinessException("Không tìm thấy phòng chiếu.");
            db.Rooms.Remove(room);
            db.SaveChanges();
        }

        public static List<Seat> BuildSeats(int roomId, int rows, int cols, int vipRows, bool lastRowCouple)
        {
            var seats = new List<Seat>();
            int normalRows = lastRowCouple ? rows - 1 : rows;

            for (int r = 0; r < normalRows; r++)
            {
                string rowLetter = ((char)('A' + r)).ToString();
                bool isVip = r >= normalRows - Math.Min(vipRows, normalRows);
                for (int c = 0; c < cols; c++)
                {
                    seats.Add(new Seat
                    {
                        RoomId = roomId,
                        SeatNumber = $"{rowLetter}{c + 1}",
                        RowIndex = r,
                        ColIndex = c,
                        SeatType = isVip ? SeatTypes.Vip : SeatTypes.Standard
                    });
                }
            }

            if (lastRowCouple)
            {
                int r = rows - 1;
                string rowLetter = ((char)('A' + r)).ToString();
                int coupleCount = cols / 2;
                for (int i = 0; i < coupleCount; i++)
                {
                    seats.Add(new Seat
                    {
                        RoomId = roomId,
                        SeatNumber = $"{rowLetter}{i + 1}",
                        RowIndex = r,
                        ColIndex = i * 2,
                        SeatType = SeatTypes.Couple
                    });
                }
            }

            return seats;
        }
    }
}
