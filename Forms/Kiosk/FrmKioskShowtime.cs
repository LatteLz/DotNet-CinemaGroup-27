using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Forms.Kiosk
{
    public partial class FrmKioskShowtime : Form
    {
        private readonly int _movieId;
        private int _selectedShowtimeId = 0;
        private readonly System.Windows.Forms.Timer _timer;
        private DateTime _selectedDate = DateTime.Today;

        public FrmKioskShowtime(int movieId)
        {
            _movieId = movieId;
            InitializeComponent();

            _timer = new System.Windows.Forms.Timer { Interval = 1000 };
            _timer.Tick += (_, _) =>
            {
                if (lblClock != null && !lblClock.IsDisposed)
                    lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _timer.Start();

            RegisterEvents();
            LoadShowtimes();
            LoadMovieLeftInfo();
            RenderDateTabs();
        }

        private void RegisterEvents()
        {
            this.VisibleChanged += (_, _) =>
            {
                if (this.Visible)
                {
                    _timer.Start();
                    LoadShowtimes();
                }
            };

            btnBack.Click += (_, _) =>
            {
                _timer.Stop();
                var home = Application.OpenForms.OfType<FrmKioskHome>().FirstOrDefault();
                if (home != null)
                {
                    home.TopMost = true;
                    home.Show();
                    home.Activate();
                }
                else
                {
                    var newHome = new FrmKioskHome();
                    newHome.TopMost = true;
                    newHome.Show();
                }
                this.Close();
            };

            btnExit.Click += (_, _) =>
            {
                _timer.Stop();
                Form? mainForm = Application.OpenForms.Cast<Form>()
                    .FirstOrDefault(f => f.Name == "FrmMain" || f.Name == "FrmRoleSelection" || f.Tag?.ToString() == "Main");
                if (mainForm != null)
                {
                    mainForm.Show();
                }
                this.Close();
            };

            btnContinue.Click += (_, _) =>
            {
                if (_selectedShowtimeId <= 0)
                {
                    MessageBox.Show("Vui lòng chọn một suất chiếu trước khi chọn ghế!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _timer.Stop();
                FrmKioskSeat frmSeat = new FrmKioskSeat(_selectedShowtimeId);
                frmSeat.TopMost = true;
                frmSeat.FormClosed += (_, _) =>
                {
                    if (!this.IsDisposed)
                    {
                        this.Show();
                        this.Activate();
                    }
                };
                frmSeat.Show();
                this.Hide();
            };
        }

        private void FilterShowtimes(string format)
        {
            using var db = new CinemaDbContext();
            var query = db.Showtimes
                .Include(st => st.Room)
                .Where(st => st.MovieId == _movieId && st.StartTime.Date == _selectedDate.Date);

            if (format == "IMAX")
            {
                query = query.Where(st => st.Room != null && st.Room.RoomName.Contains("IMAX"));
            }
            else if (format == "Standard")
            {
                query = query.Where(st => st.Room == null || !st.Room.RoomName.Contains("IMAX"));
            }

            _ = query.ToList();
            LoadShowtimes();
        }

        private void LoadMovieLeftInfo()
        {
            using var db = new CinemaDbContext();
            var movie = db.Movies.FirstOrDefault(m => m.MovieId == _movieId);
            if (movie == null) return;

            lblMovieTitle.Text = movie.Title.ToUpper();

            PictureBox picLeftPoster = new PictureBox
            {
                Size = new Size(350, 240),
                Location = new Point(24, 65),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ColorTranslator.FromHtml("#262a2f")
            };

            string relativePath = movie.PosterPath ?? "";
            if (!string.IsNullOrEmpty(relativePath))
            {
                string cleanPath = relativePath.TrimStart('/', '\\');
                string fullPath = Path.Combine(Application.StartupPath, cleanPath);

                if (!File.Exists(fullPath))
                {
                    fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", cleanPath);
                }

                if (File.Exists(fullPath))
                {
                    try
                    {
                        using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                        picLeftPoster.Image = Image.FromStream(stream);
                    }
                    catch
                    {
                    }
                }
            }

            var ageRatingProp = movie.GetType().GetProperty("AgeRating") ?? movie.GetType().GetProperty("Rating");
            string ageRating = ageRatingProp?.GetValue(movie)?.ToString() ?? "T16";
            if (string.IsNullOrWhiteSpace(ageRating)) ageRating = "T16";

            Color badgeColor = ageRating.StartsWith("P") ? ColorTranslator.FromHtml("#10B981") : ColorTranslator.FromHtml("#EF4444");

            Label lblBadgeAge = new Label
            {
                Text = $"{ageRating.ToUpper()} • THEO QUY ĐỊNH RẠP",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = badgeColor,
                Padding = new Padding(8, 4, 8, 4),
                Location = new Point(24, 320),
                AutoSize = true
            };

            Label lblDetail = new Label
            {
                Text = $"Thể loại: {movie.Genre}\nThời lượng: {movie.Duration} phút\nCông nghệ: IMAX LASER 4K\nĐánh giá: 8.8/10 ★",
                Font = new Font("Segoe UI", 10F),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                Location = new Point(24, 360),
                AutoSize = true
            };

            Label lblDescHeader = new Label
            {
                Text = "TÓM TẮT NỘI DUNG",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(24, 480),
                AutoSize = true
            };

            var descProperty = movie.GetType().GetProperty("Description");
            string description = descProperty?.GetValue(movie)?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(description))
            {
                description = $"Bộ phim {movie.Title} hứa hẹn mang đến những giây phút giải trí tuyệt vời với công nghệ âm thanh sống động.";
            }

            Label lblDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                Location = new Point(24, 510),
                Size = new Size(350, 420),
                AutoEllipsis = true
            };

            pnlLeftMovieInfo.Controls.Add(picLeftPoster);
            pnlLeftMovieInfo.Controls.Add(lblBadgeAge);
            pnlLeftMovieInfo.Controls.Add(lblDetail);
            pnlLeftMovieInfo.Controls.Add(lblDescHeader);
            pnlLeftMovieInfo.Controls.Add(lblDesc);
        }

        private void RenderDateTabs()
        {
            pnlDates.Controls.Clear();
            DateTime today = DateTime.Today;

            for (int i = 0; i < 4; i++)
            {
                DateTime targetDate = today.AddDays(i);
                string dateTitle;

                if (i == 0) dateTitle = $"HÔM NAY {targetDate:dd/MM}";
                else if (i == 1) dateTitle = $"NGÀY MAI {targetDate:dd/MM}";
                else
                {
                    string dayOfWeekVi = targetDate.ToString("dddd", new CultureInfo("vi-VN")).ToUpper();
                    dateTitle = $"{dayOfWeekVi} {targetDate:dd/MM}";
                }

                Button btnDate = new Button
                {
                    Text = dateTitle,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = targetDate.Date == _selectedDate.Date ? ColorTranslator.FromHtml("#101418") : ColorTranslator.FromHtml("#c1c7cf"),
                    BackColor = targetDate.Date == _selectedDate.Date ? ColorTranslator.FromHtml("#38bdf8") : ColorTranslator.FromHtml("#181c20"),
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(180, 60),
                    Margin = new Padding(0, 0, 16, 0),
                    Cursor = Cursors.Hand,
                    Tag = targetDate
                };
                btnDate.FlatAppearance.BorderSize = 0;

                btnDate.Click += (s, _) =>
                {
                    if (s is Button clickedBtn && clickedBtn.Tag is DateTime dt)
                    {
                        _selectedDate = dt;
                        RenderDateTabs();
                        LoadShowtimes();
                    }
                };

                pnlDates.Controls.Add(btnDate);
            }
        }

        private void LoadShowtimes()
        {
            pnlShowtimes.Controls.Clear();
            _selectedShowtimeId = 0;
            btnContinue.Enabled = false;
            btnContinue.BackColor = ColorTranslator.FromHtml("#282C31");
            lblSelectedShowtimeSummary.Text = "SUẤT CHIẾU ĐANG CHỌN: (Vui lòng chọn suất chiếu)";

            using var db = new CinemaDbContext();
            var showtimes = db.Showtimes
                .Include(s => s.Room)
                .Where(s => s.MovieId == _movieId && s.StartTime.Date == _selectedDate.Date)
                .OrderBy(s => s.StartTime)
                .ToList();

            if (!showtimes.Any())
            {
                Label lblEmpty = new Label
                {
                    Text = $"Không có suất chiếu nào vào ngày {_selectedDate:dd/MM/yyyy}.",
                    ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                    Font = new Font("Segoe UI", 12F),
                    AutoSize = true,
                    Margin = new Padding(10),
                };
                pnlShowtimes.Controls.Add(lblEmpty);
                return;
            }

            Label lblRoomHeader = new Label
            {
                Text = "IMAX LASER 4K  •  Phòng Chiếu 01 (Màn hình vòm 26m) - Giá vé từ 160.000 đ",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 16)
            };
            pnlShowtimes.Controls.Add(lblRoomHeader);
            pnlShowtimes.SetFlowBreak(lblRoomHeader, true);

            DateTime now = DateTime.Now;

            foreach (var st in showtimes)
            {
                bool isPast = st.StartTime < now;
                int totalSeats = db.Seats.Count(seat => seat.RoomId == st.RoomId);
                if (totalSeats == 0) totalSeats = 140;

                int bookedSeats = db.Tickets.Count(t => t.ShowtimeId == st.ShowtimeId);
                int heldSeats = db.SeatHolds.Count(h => h.ShowtimeId == st.ShowtimeId && h.ExpiresAt > now);
                int availableSeats = Math.Max(0, totalSeats - bookedSeats - heldSeats);
                bool isSoldOut = availableSeats == 0;
                bool isDisabled = isPast || isSoldOut;

                string statusText = isPast
                    ? $"{st.StartTime:HH:mm}\n(Đã chiếu)"
                    : isSoldOut
                        ? $"{st.StartTime:HH:mm}\nPhòng {st.RoomId}\n(Hết ghế)"
                        : $"{st.StartTime:HH:mm}\nPhòng {st.RoomId}\nCòn {availableSeats} ghế";

                Button btnTime = new Button
                {
                    Text = statusText,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = isDisabled ? ColorTranslator.FromHtml("#64748B") : Color.White,
                    BackColor = isDisabled ? ColorTranslator.FromHtml("#0F172A") : ColorTranslator.FromHtml("#181c20"),
                    Size = new Size(160, 90),
                    Margin = new Padding(0, 0, 20, 20),
                    FlatStyle = FlatStyle.Flat,
                    Enabled = !isDisabled,
                    Cursor = isDisabled ? Cursors.No : Cursors.Hand,
                    Tag = st.ShowtimeId
                };
                btnTime.FlatAppearance.BorderColor = isDisabled ? ColorTranslator.FromHtml("#1E293B") : ColorTranslator.FromHtml("#40484f");

                if (!isDisabled)
                {
                    btnTime.Click += (_, _) =>
                    {
                        foreach (Control ctrl in pnlShowtimes.Controls)
                        {
                            if (ctrl is Button b && b.Enabled)
                            {
                                b.BackColor = ColorTranslator.FromHtml("#181c20");
                                b.ForeColor = Color.White;
                            }
                        }
                        btnTime.BackColor = ColorTranslator.FromHtml("#ffb961");
                        btnTime.ForeColor = ColorTranslator.FromHtml("#101418");
                        _selectedShowtimeId = st.ShowtimeId;

                        string roomLabel = st.Room?.RoomName ?? $"Phòng Chiếu {st.RoomId}";
                        lblSelectedShowtimeSummary.Text = $"SUẤT CHIẾU ĐANG CHỌN: {roomLabel} • {st.StartTime:HH:mm}";
                        btnContinue.Enabled = true;
                        btnContinue.BackColor = ColorTranslator.FromHtml("#ffb961");
                    };
                }

                pnlShowtimes.Controls.Add(btnTime);
            }
        }
    }
}