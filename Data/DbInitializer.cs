using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Models;
using CinemaManagement.Helpers;

namespace CinemaManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CinemaDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                PasswordHelper.CreatePasswordHash("password123", out string hash, out string salt);

                context.Users.AddRange(
                    new User
                    {
                        Username = "admin.director@cineluxe.vn",
                        PasswordHash = hash,
                        Salt = salt,
                        Role = "Admin"
                    },
                    new User
                    {
                        Username = "staff.pos@cineluxe.vn",
                        PasswordHash = hash,
                        Salt = salt,
                        Role = "Staff"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Movies.Any())
            {
                context.Movies.AddRange(
                    new Movie
                    {
                        Title = "Spiderman: No Way Home", 
                        Genre = "Hành động / Viễn tưởng", 
                        Duration = 148, 
                        AgeRating = "T13", 
                        PosterPath = "",
                        Description = "Phim siêu anh hùng bom tấn Marvel"
                    },
                    new Movie
                    {
                        Title = "Lật Mặt 7: Một Điều Ước", 
                        Genre = "Gia đình / Tâm lý", 
                        Duration = 138, 
                        AgeRating = "K", 
                        PosterPath = "",
                        Description = "Phim điện ảnh Việt Nam xuất sắc"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Rooms.Any())
            {
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
                            Price = (r == 'A' || r == 'B') ? 90000 : 60000
                        });
                    }
                }
                context.SaveChanges();
            }

            if (!context.Showtimes.Any())
            {
                var firstMovie = context.Movies.FirstOrDefault();
                var firstRoom = context.Rooms.FirstOrDefault();

                if (firstMovie != null && firstRoom != null)
                {
                    context.Showtimes.AddRange(
                        new Showtime
                        {
                            MovieId = firstMovie.MovieId,
                            RoomId = firstRoom.RoomId,
                            StartTime = DateTime.Now.Date.AddHours(14),
                            TicketPrice = 75000
                        },
                        new Showtime
                        {
                            MovieId = firstMovie.MovieId,
                            RoomId = firstRoom.RoomId,
                            StartTime = DateTime.Now.Date.AddHours(19).AddMinutes(30),
                            TicketPrice = 90000
                        }
                    );
                    context.SaveChanges();
                }
            }
            if (!context.ComboItems.Any())
            {
                context.ComboItems.AddRange(
                    new ComboItem { ComboName = "My Combo (1 Bắp + 1 Nước)", Price = 70000, ImagePath = "" },
                    new ComboItem { ComboName = "CGV Combo (1 Bắp + 2 Nước)", Price = 90000, ImagePath = "" },
                    new ComboItem { ComboName = "Combo Couple IMAX", Price = 129000m, ImagePath = "" },
                    new ComboItem { ComboName = "Bắp Ngọt Lớn", Price = 55000m, ImagePath = "" }
                );
                context.SaveChanges();
            }
        }
    }
}