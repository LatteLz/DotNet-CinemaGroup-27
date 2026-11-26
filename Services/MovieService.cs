using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class MovieService
    {
        public List<Movie> GetAll()
        {
            using var db = DbFactory.Create();
            return db.Movies.AsNoTracking().OrderBy(m => m.Title).ToList();
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            await using var db = DbFactory.Create();
            return await db.Movies.AsNoTracking().OrderBy(m => m.Title).ToListAsync();
        }

        public Movie? GetById(int movieId)
        {
            using var db = DbFactory.Create();
            return db.Movies.AsNoTracking().FirstOrDefault(m => m.MovieId == movieId);
        }

        public List<Movie> GetMoviesForKiosk(string filter)
        {
            using var db = DbFactory.Create();
            return KioskMoviesQuery(db, filter).ToList();
        }

        public async Task<List<Movie>> GetMoviesForKioskAsync(string filter)
        {
            await using var db = DbFactory.Create();
            return await KioskMoviesQuery(db, filter).ToListAsync();
        }

        private static IQueryable<Movie> KioskMoviesQuery(CinemaDbContext db, string filter)
        {
            DateTime cutoff = DateTime.Now.AddMinutes(-ShowtimeService.SellUntilMinutesAfterStart);
            DateTime tomorrow = DateTime.Today.AddDays(1);
            DateTime windowEnd = DateTime.Today.AddDays(ShowtimeService.SalesWindowDays);

            var upcoming = db.Showtimes.Where(s => s.StartTime > cutoff && s.StartTime < windowEnd);

            switch (filter)
            {
                case "Today":
                    upcoming = upcoming.Where(s => s.StartTime < tomorrow);
                    break;
                case "IMAX":
                    upcoming = upcoming.Where(s => s.Room.RoomType.Contains("IMAX") || s.Room.RoomName.Contains("IMAX"));
                    break;
                case "Standard":
                    upcoming = upcoming.Where(s => !s.Room.RoomType.Contains("IMAX") && !s.Room.RoomName.Contains("IMAX")
                                                   && s.Room.RoomType != RoomTypes.ScreenX);
                    break;
                default:
                    break;
            }

            var movieIds = upcoming.Select(s => s.MovieId).Distinct();
            return db.Movies.AsNoTracking()
                .Where(m => movieIds.Contains(m.MovieId))
                .OrderBy(m => m.Title);
        }

        public void Add(Movie movie)
        {
            Validate(movie);
            using var db = DbFactory.Create();
            EnsureUniqueTitle(db, movie);
            db.Movies.Add(movie);
            db.SaveChanges();
        }

        public void Update(Movie movie)
        {
            Validate(movie);
            using var db = DbFactory.Create();
            var existing = db.Movies.Find(movie.MovieId)
                           ?? throw new BusinessException("Không tìm thấy phim cần cập nhật.");
            EnsureUniqueTitle(db, movie);

            bool durationChanged = existing.Duration != movie.Duration;
            db.Entry(existing).CurrentValues.SetValues(movie);

            if (durationChanged)
            {
                var futureShowtimes = db.Showtimes
                    .Where(s => s.MovieId == movie.MovieId && s.StartTime > DateTime.Now)
                    .ToList();
                foreach (var st in futureShowtimes)
                {
                    DateTime newEnd = ShowtimeService.CalculateEndTime(st.StartTime, movie.Duration);
                    var conflict = db.Showtimes
                        .Where(o => o.RoomId == st.RoomId && o.ShowtimeId != st.ShowtimeId
                                    && o.StartTime < newEnd && o.EndTime > st.StartTime)
                        .Select(o => new { o.StartTime, o.Room.RoomName })
                        .FirstOrDefault();
                    if (conflict != null)
                        throw new BusinessException(
                            $"Không thể đổi thời lượng: suất {st.StartTime:HH:mm dd/MM} sẽ kéo dài đè lên suất " +
                            $"{conflict.StartTime:HH:mm dd/MM} ở {conflict.RoomName}. Hãy dời lịch chiếu trước.");
                    st.EndTime = newEnd;
                }
            }

            db.SaveChanges();
        }

        public void Delete(int movieId)
        {
            using var db = DbFactory.Create();

            if (db.Showtimes.Any(s => s.MovieId == movieId))
                throw new BusinessException("Phim này đã có suất chiếu nên không thể xóa. Hãy xóa các suất chiếu của phim trước.");

            var movie = db.Movies.Find(movieId) ?? throw new BusinessException("Không tìm thấy phim cần xóa.");
            db.Movies.Remove(movie);
            db.SaveChanges();
        }

        private static void EnsureUniqueTitle(CinemaDbContext db, Movie movie)
        {
            string title = movie.Title.ToLower();
            if (db.Movies.Any(m => m.MovieId != movie.MovieId && m.Title.ToLower() == title))
                throw new BusinessException($"Phim \"{movie.Title}\" đã có trong danh mục.");
        }

        private static void Validate(Movie movie)
        {
            if (string.IsNullOrWhiteSpace(movie.Title))
                throw new BusinessException("Tên phim không được để trống.");
            if (movie.Duration <= 0 || movie.Duration > 400)
                throw new BusinessException("Thời lượng phim phải từ 1 đến 400 phút.");
            movie.Title = movie.Title.Trim();
        }
    }
}
