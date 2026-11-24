using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CinemaManagement.Data
{
    public static class DbInitializer
    {
        public const string DefaultAdminUsername = "admin";
        public const string DefaultAdminPassword = "admin123";
        public const string DefaultStaffUsername = "staff";
        public const string DefaultStaffPassword = "staff123";

        public static void Initialize()
        {
            using var context = DbFactory.Create();
            DeleteIfCreatedWithoutMigrations(context);
            context.Database.Migrate();

            SeedUsers(context);
            SeedCombos(context);
            SeedMovies(context);
            SeedRoomsAndSeats(context);
            SeedShowtimes(context);
        }

        private static void DeleteIfCreatedWithoutMigrations(CinemaDbContext context)
        {
            var creator = context.Database.GetService<IRelationalDatabaseCreator>();
            if (creator.Exists() && creator.HasTables() && !context.Database.GetAppliedMigrations().Any())
                context.Database.EnsureDeleted();
        }

        private static void SeedUsers(CinemaDbContext context)
        {
            if (context.Users.Any()) return;

            PasswordHelper.CreatePasswordHash(DefaultAdminPassword, out string adminHash, out string adminSalt);
            PasswordHelper.CreatePasswordHash(DefaultStaffPassword, out string staffHash, out string staffSalt);

            context.Users.AddRange(
                new User
                {
                    Username = DefaultAdminUsername,
                    Fullname = "Quản Trị Viên",
                    Email = "admin@cineluxe.vn",
                    Role = Roles.Admin,
                    PasswordHash = adminHash,
                    Salt = adminSalt,
                    IsActive = true,
                    CreateAt = DateTime.Now
                },
                new User
                {
                    Username = DefaultStaffUsername,
                    Fullname = "Nhân Viên Thu Ngân",
                    Email = "staff@cineluxe.vn",
                    Role = Roles.Staff,
                    PasswordHash = staffHash,
                    Salt = staffSalt,
                    IsActive = true,
                    CreateAt = DateTime.Now
                });
            context.SaveChanges();
        }

        private static void SeedCombos(CinemaDbContext context)
        {
            if (context.ComboItems.Any()) return;

            context.ComboItems.AddRange(
                new ComboItem { ComboName = "Combo Solo", Description = "1 Bắp ngọt (L) + 1 Nước ngọt (L)", Price = 79000m, ImagePath = "Image/solocb.png" },
                new ComboItem { ComboName = "Combo Couple", Description = "1 Bắp ngọt (XL) + 2 Nước ngọt (L)", Price = 119000m, ImagePath = "Image/cpcb.png" },
                new ComboItem { ComboName = "Combo Family", Description = "2 Bắp (XL) + 4 Nước ngọt (L)", Price = 199000m, ImagePath = "Image/fmcb.png" });
            context.SaveChanges();
        }

        private static void SeedMovies(CinemaDbContext context)
        {
            if (context.Movies.Any()) return;

            context.Movies.AddRange(
                new Movie { Title = "Thám Tử Lừng Danh Conan", OriginalTitle = "Detective Conan", Duration = 110, Genre = "Hoạt hình, Trinh thám", AgeRating = "T13", Country = "Nhật Bản", PosterPath = "Image/Conan.jpg", Description = "Conan và những người bạn đối mặt với một vụ án bí ẩn mới." },
                new Movie { Title = "Người Nhện", OriginalTitle = "Spider-Man", Duration = 135, Genre = "Hành động, Phiêu lưu", AgeRating = "T13", Country = "Mỹ", PosterPath = "Image/Spider.jpg", Description = "Cuộc phiêu lưu mới của người hùng mạng nhện." },
                new Movie { Title = "Deadpool & Wolverine", OriginalTitle = "Deadpool & Wolverine", Duration = 128, Genre = "Hành động, Hài", AgeRating = "T18", Country = "Mỹ", PosterPath = "Image/dpw.jpg", Description = "Hai dị nhân bất đắc dĩ phải hợp tác." },
                new Movie { Title = "Exhuma: Quật Mộ Trùng Ma", OriginalTitle = "Exhuma", Duration = 134, Genre = "Kinh dị", AgeRating = "T18", Country = "Hàn Quốc", PosterPath = "Image/exhuma.jpg", Description = "Một nhóm pháp sư khai quật ngôi mộ cổ và đánh thức điều đáng sợ." },
                new Movie { Title = "Godzilla x Kong: Đế Chế Mới", OriginalTitle = "Godzilla x Kong: The New Empire", Duration = 115, Genre = "Hành động, Viễn tưởng", AgeRating = "T13", Country = "Mỹ", PosterPath = "Image/gxk.jpg", Description = "Hai siêu quái vật hợp lực trước mối đe dọa mới." },
                new Movie { Title = "Kung Fu Panda 4", OriginalTitle = "Kung Fu Panda 4", Duration = 94, Genre = "Hoạt hình, Hài", AgeRating = "P", Country = "Mỹ", PosterPath = "Image/kfp4.jpg", Description = "Gấu Po tìm người kế nhiệm Thần Long Đại Hiệp." },
                new Movie { Title = "Lật Mặt 7: Một Điều Ước", OriginalTitle = "Face Off 7: One Wish", Duration = 138, Genre = "Gia đình, Tâm lý", AgeRating = "K", Country = "Việt Nam", PosterPath = "Image/latmat7.jpg", Description = "Câu chuyện cảm động về tình mẫu tử." },
                new Movie { Title = "Trại Buôn Người", Duration = 105, Genre = "Tâm lý, Hình sự", AgeRating = "T16", Country = "Việt Nam", PosterPath = "Image/Traibuonnguoi.jpg", Description = "Một vụ án đường dây buôn người được phơi bày." });
            context.SaveChanges();
        }

        private static void SeedRoomsAndSeats(CinemaDbContext context)
        {
            if (context.Rooms.Any()) return;

            var rooms = new List<(Room Room, int VipRows, bool CoupleRow)>
            {
                (new Room { RoomName = "Phòng 01 - IMAX", RoomType = RoomTypes.Imax, RowCount = 8, ColCount = 12 }, 2, true),
                (new Room { RoomName = "Phòng 02", RoomType = RoomTypes.Standard, RowCount = 8, ColCount = 10 }, 2, true),
                (new Room { RoomName = "Phòng 03 - VIP", RoomType = RoomTypes.Vip, RowCount = 6, ColCount = 8 }, 6, false)
            };

            foreach (var (room, vipRows, coupleRow) in rooms)
            {
                context.Rooms.Add(room);
                context.SaveChanges();
                context.Seats.AddRange(RoomService.BuildSeats(room.RoomId, room.RowCount, room.ColCount, vipRows, coupleRow));
                context.SaveChanges();
            }
        }

        private static void SeedShowtimes(CinemaDbContext context)
        {
            DateTime now = DateTime.Now;
            if (context.Showtimes.Any(s => s.StartTime > now)) return;

            var movies = context.Movies.OrderBy(m => m.MovieId).ToList();
            var rooms = context.Rooms.OrderBy(r => r.RoomId).ToList();
            if (movies.Count == 0 || rooms.Count == 0) return;

            int movieCursor = 0;
            for (int day = 0; day < ShowtimeService.SalesWindowDays; day++)
            {
                DateTime date = DateTime.Today.AddDays(day);
                DateTime nextDate = date.AddDays(1);
                if (context.Showtimes.Any(s => s.StartTime >= date && s.StartTime < nextDate)) continue;

                for (int r = 0; r < rooms.Count; r++)
                {
                    DateTime start = date.AddHours(9).AddMinutes(r * 30);
                    decimal basePrice = RoomTypes.SuggestedPrice(rooms[r].RoomType);

                    while (true)
                    {
                        var movie = movies[movieCursor % movies.Count];
                        DateTime end = ShowtimeService.CalculateEndTime(start, movie.Duration);
                        if (end.Date > date) break;

                        context.Showtimes.Add(new Showtime
                        {
                            MovieId = movie.MovieId,
                            RoomId = rooms[r].RoomId,
                            StartTime = start,
                            EndTime = end,
                            Price = start.Hour >= 17 ? basePrice + 10000m : basePrice
                        });

                        movieCursor++;
                        start = RoundUpToQuarter(end);
                    }
                }
            }
            context.SaveChanges();
        }

        private static DateTime RoundUpToQuarter(DateTime t)
        {
            int extra = (15 - t.Minute % 15) % 15;
            var rounded = t.AddMinutes(extra);
            return new DateTime(rounded.Year, rounded.Month, rounded.Day, rounded.Hour, rounded.Minute, 0);
        }
    }
}
