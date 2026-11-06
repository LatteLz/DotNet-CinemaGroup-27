using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CinemaDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Movies.Any()) return;

            // 1. Thêm phim
            context.Movies.AddRange(
                new Movie
                {
                    Title = "Spiderman", 
                    Genre = "Điện ảnh", 
                    Duration = 120, 
                    AgeRating = "T18", 
                    PosterPath = "",
                    Description = "Phim điện ảnh do Marvel sản xuất"
                },
                new Movie
                {
                    Title = "Trại Buôn Người", 
                    Genre = "Hành Động", 
                    Duration = 135, 
                    AgeRating = "T18", 
                    PosterPath = "",
                    Description = "Phim điện ảnh Việt Nam"
                }
            );
            context.SaveChanges();

            // 2. Thêm phòng chiếu (Nhớ tham khảo của CGV nghe, coi vị trí ghế nữa nha 2 b)
            var room = new Room { RoomName = "Phòng chiếu 01 (IMAX)" };
            context.Rooms.Add(room);
            context.SaveChanges();

            char[] rows = { 'A', 'B', 'C', 'D', 'E' };
            foreach (var r in rows)
            {
                for (int c = 1; c <= 8; c++)
                {
                    context.Seats.Add(new Seat
                    {
                        RoomId = room.RoomId,
                        SeatNumber = $"{r}{c}",
                        SeatType = (r == 'A' || r == 'B') ? "VIP" : "Normal",
                        Price = (r == 'A' || r == 'B') ? 90000 : 60000 // Thêm giá ghế cho phù hợp với Seat model
                    });
                }
            }
            context.SaveChanges();

            // 3. Thêm tài khoản admin mặc định
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                context.Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = "123456",
                    Fullname = "Quản Trị Viên",
                    Role = "Admin",
                    IsActive = true
                });
                context.SaveChanges();
            }

            //  Combo bắp nước (Nhớ tham khảo của CGV nha)
            context.ComboItems.AddRange(
                new ComboItem { ComboName = "My Combo (1 Bắp + 1 Nước)", Price = 70000, ImagePath = "" },
                new ComboItem { ComboName = "CGV Combo (1 Bắp + 2 Nước)", Price = 90000, ImagePath = "" }
            );
            context.SaveChanges();
        }
    }
}