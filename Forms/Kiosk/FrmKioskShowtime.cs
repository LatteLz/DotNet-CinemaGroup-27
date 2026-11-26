using System.Globalization;
using CinemaManagement.Helpers;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Kiosk
{
    public partial class FrmKioskShowtime : Form
    {
        private readonly int _movieId;
        private int _selectedShowtimeId = 0;
        private readonly System.Windows.Forms.Timer _timer;
        private DateTime _selectedDate = DateTime.Today;
        private readonly MovieService _movieService = new();
        private readonly ShowtimeService _showtimeService = new();

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
            WindowLayout.ScaleToScreen(this);
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
                KioskFlow.ExitKiosk(this);
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

        private void LoadMovieLeftInfo()
        {
            Models.Movie? movie;
            try
            {
                movie = _movieService.GetById(_movieId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được thông tin phim: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (movie == null) return;

            lblMovieTitle.Text = movie.Title.ToUpper();

            PictureBox picLeftPoster = new PictureBox
            {
                Size = new Size(350, 240),
                Location = new Point(24, 65),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ColorTranslator.FromHtml("#262a2f"),
                Image = ImageHelper.Load(movie.PosterPath)
            };

            string ageRating = string.IsNullOrWhiteSpace(movie.AgeRating) ? "P" : movie.AgeRating;
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

            string details = $"Thể loại: {movie.Genre}\nThời lượng: {movie.Duration} phút";
            if (!string.IsNullOrWhiteSpace(movie.Country)) details += $"\nQuốc gia: {movie.Country}";
            if (!string.IsNullOrWhiteSpace(movie.Director)) details += $"\nĐạo diễn: {movie.Director}";

            Label lblDetail = new Label
            {
                Text = details,
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

            Label lblDesc = new Label
            {
                Text = string.IsNullOrWhiteSpace(movie.Description) ? "Chưa có mô tả." : movie.Description,
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

            for (int i = 0; i < ShowtimeService.SalesWindowDays; i++)
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

            List<ShowtimeListItemDto> showtimes;
            try
            {
                showtimes = _showtimeService.GetShowtimesForMovie(_movieId, _selectedDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được suất chiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            foreach (var roomGroup in showtimes.GroupBy(s => new { s.RoomId, s.RoomName, s.RoomType }))
            {
                decimal minPrice = roomGroup.Min(s => s.Price);
                Label lblRoomHeader = new Label
                {
                    Text = $"{roomGroup.Key.RoomName.ToUpper()}  •  {roomGroup.Key.RoomType} - Giá vé từ {minPrice:N0} đ",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = ColorTranslator.FromHtml("#ffb961"),
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 16)
                };
                pnlShowtimes.Controls.Add(lblRoomHeader);
                pnlShowtimes.SetFlowBreak(lblRoomHeader, true);

                Control? lastButton = null;
                foreach (var st in roomGroup)
                {
                    bool isPast = !ShowtimeService.IsSellable(st.StartTime);
                    bool started = !isPast && st.StartTime <= DateTime.Now;
                    bool isSoldOut = st.AvailableSeats == 0;
                    bool isDisabled = isPast || isSoldOut;

                    string statusText = isPast
                        ? $"{st.StartTime:HH:mm}\n(Hết giờ bán)"
                        : isSoldOut
                            ? $"{st.StartTime:HH:mm}\n(Hết ghế)"
                            : started
                                ? $"{st.StartTime:HH:mm}\n(Đã bắt đầu)\nCòn {st.AvailableSeats}/{st.TotalSeats} ghế"
                                : $"{st.StartTime:HH:mm}\nCòn {st.AvailableSeats}/{st.TotalSeats} ghế";

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
                        var selected = st;
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
                            _selectedShowtimeId = selected.ShowtimeId;

                            lblSelectedShowtimeSummary.Text = $"SUẤT CHIẾU ĐANG CHỌN: {selected.RoomName} • {selected.StartTime:HH:mm} • {selected.Price:N0} đ/ghế thường";
                            btnContinue.Enabled = true;
                            btnContinue.BackColor = ColorTranslator.FromHtml("#ffb961");
                        };
                    }

                    pnlShowtimes.Controls.Add(btnTime);
                    lastButton = btnTime;
                }

                if (lastButton != null) pnlShowtimes.SetFlowBreak(lastButton, true);
            }
        }
    }
}
