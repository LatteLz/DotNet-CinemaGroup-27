using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;
using Timer = System.Windows.Forms.Timer;
using System.IO;
using System.Globalization;
namespace CinemaManagement.Forms.Kiosk;

public class FrmKioskShowtime : Form
{
     private int _movieId;
    private FlowLayoutPanel pnlShowtimes;
    private Label lblMovieTitle;
    private Button btnBack;
    private Button btnContinue;
    private int _selectedShowtimeId = 0;
    private Panel pnlHeader;
    private Panel pnlLeftMovieInfo;
    private Panel pnlRightShowtimes;
    private Panel pnlBottomBar;
    private FlowLayoutPanel pnlDates;
    private Label lblSelectedShowtimeSummary;
    private Label lblClock;
    private System.Windows.Forms.Timer _timer;
    private DateTime _selectedDate = DateTime.Today;
    public FrmKioskShowtime (int movieId)
    {
        _movieId = movieId;
        InitializeComponent();
        LoadShowtimes();
        LoadMovieLeftInfo();
        RenderDateTabs();
    }
    private void InitializeComponent()
    {
        this.Text = "CineLuxe Kiosk - Chọn suất chiếu";
        this.Size = new Size(1920,1080);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = ColorTranslator.FromHtml("#101418");
        pnlHeader = new Panel
        {
            Size = new Size(1920, 80),
            Location = new Point(0, 0),
            BackColor = ColorTranslator.FromHtml("#181c20")
        };

        Label lblLogo = new Label
        {
            Text = "CINELUXE KIOSK",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
            Location = new Point(24, 26),
            AutoSize = true
        };
            
        Label lblStep1 = new Label
        {
            Text = "1. Chọn Phim",
            Font = new Font("Segoe UI", 11F),
            ForeColor = ColorTranslator.FromHtml("#8b9198"),
            Location = new Point(600, 28),
            AutoSize = true
        };

        Label lblStep2 = new Label
        {
            Text = "2. Chọn Suất Chiếu",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = ColorTranslator.FromHtml("#1a5276"),
            Padding = new Padding(16, 8, 16, 8),
            Location = new Point(760, 20),
            AutoSize = true
        };

        lblClock = new Label
        {
            Text = DateTime.Now.ToString("HH:mm:ss"),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"),
            Location = new Point(1530, 26),
            AutoSize = true
        };
        btnBack = new Button
        {
            Text = "← Quay lại ",
            Font = new Font("Segoe UI",9.5F,FontStyle.Bold),
            ForeColor =  ColorTranslator.FromHtml("#94A3B8"),
            BackColor = ColorTranslator.FromHtml("#262a2f"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(110,38),
            Location = new Point(1660,21),
            Cursor = Cursors.Hand
        };
        btnBack.FlatAppearance.BorderSize = 0;
        btnBack.Click += (s, e) =>
        {
            FrmKioskHome home = new FrmKioskHome();
            home.Show();
            this.Close();
        };
        Button btnExit = new Button
        {
            Text = "✕ THOÁT",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = ColorTranslator.FromHtml("#ef4444"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(100, 38),
            Location = new Point(1780, 21),
            Cursor = Cursors.Hand
        };
        btnExit.FlatAppearance.BorderSize = 0;
        btnExit.Click += (s, e) =>
        {
            Form mainForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.Name == "FrmMain" || f.Name == "FrmRoleSelection" || f.Tag?.ToString() == "Main");
            if (mainForm != null)
            {
                mainForm.Show();
            }
            this.Close();
        }; 
        pnlHeader.Controls.Add(lblLogo);
        pnlHeader.Controls.Add(lblStep1);
        pnlHeader.Controls.Add(lblStep2);
        pnlHeader.Controls.Add(lblClock);
        pnlHeader.Controls.Add(btnBack);
        pnlHeader.Controls.Add(btnExit);
        
        pnlLeftMovieInfo = new Panel
        {
            Size = new Size(400, 976),
            Location = new Point(24, 96),
            BackColor = ColorTranslator.FromHtml("#1c2024")
        };
        lblMovieTitle = new Label
        {
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#F59E0B"), 
            Location = new Point(24, 24),
            AutoSize = true,
            MaximumSize = new Size(350, 0)
        };
        pnlLeftMovieInfo.Controls.Add(lblMovieTitle);
        
        pnlRightShowtimes = new Panel
        {
            Size = new Size(1444, 876),
            Location = new Point(452, 96),
            BackColor = ColorTranslator.FromHtml("#101418")
        };
        
        pnlDates = new FlowLayoutPanel
        {
            Size = new Size(1444, 76),
            Location = new Point(0, 0),
            BackColor = ColorTranslator.FromHtml("#101418")
        };
        pnlShowtimes = new FlowLayoutPanel
        {
            Location = new Point(0, 92),
            Size = new Size(1444, 784),
            AutoScroll = true,
            Padding = new Padding(24),
            BackColor = ColorTranslator.FromHtml("#1c2024")
        };
        pnlRightShowtimes.Controls.Add(pnlDates);
        pnlRightShowtimes.Controls.Add(pnlShowtimes);
        
        pnlBottomBar = new Panel
        {
            Size = new Size(1444, 84),
            Location = new Point(452, 988),
            BackColor = ColorTranslator.FromHtml("#0b0f12")
        };

        lblSelectedShowtimeSummary = new Label
        {
            Text = "SUẤT CHIẾU ĐANG CHỌN: (Chưa chọn suất chiếu)",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
            Location = new Point(24, 30),
            AutoSize = true
        };
        btnContinue = new Button
        {
            Text = "TIẾP TỤC CHỌN GHẾ →",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#101418"),
            BackColor = ColorTranslator.FromHtml("#282C31"), 
            FlatStyle = FlatStyle.Flat,
            Size = new Size(260, 52),
            Location = new Point(1160, 16),
            Enabled = false,
            Cursor = Cursors.Hand
        };
        btnContinue.FlatAppearance.BorderSize = 0;
        btnContinue.Click += (s, e) =>
        {
            if (_selectedShowtimeId > 0)
            {
                FrmKioskSeat frmSeat = new FrmKioskSeat(_selectedShowtimeId);
                frmSeat.Show();
                this.Hide();
            }
        };
        pnlBottomBar.Controls.Add(lblSelectedShowtimeSummary);
        pnlBottomBar.Controls.Add(btnContinue);

        this.Controls.Add(pnlHeader);
        this.Controls.Add(pnlLeftMovieInfo);
        this.Controls.Add(pnlRightShowtimes);
        this.Controls.Add(pnlBottomBar);
        this.ResumeLayout(false);
        
        _timer = new System.Windows.Forms.Timer { Interval = 1000 };
        _timer.Tick += (s, e) => {
            if (lblClock != null && !lblClock.IsDisposed)
                lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        };
        _timer.Start();
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
        var posterProperty = movie.GetType().GetProperty("PosterUrl") ?? movie.GetType().GetProperty("ImagePath");
        string posterPath = posterProperty?.GetValue(movie)?.ToString();

        if (!string.IsNullOrEmpty(posterPath) && File.Exists(posterPath))
        {
            try { picLeftPoster.Image = Image.FromFile(posterPath); } catch { }
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
        string description = descProperty?.GetValue(movie)?.ToString();
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
            string dateTitle = "";

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
                ForeColor = (targetDate.Date == _selectedDate.Date) ? ColorTranslator.FromHtml("#101418") : ColorTranslator.FromHtml("#c1c7cf"),
                BackColor = (targetDate.Date == _selectedDate.Date) ? ColorTranslator.FromHtml("#38bdf8") : ColorTranslator.FromHtml("#181c20"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 60),
                Margin = new Padding(0, 0, 16, 0),
                Cursor = Cursors.Hand,
                Tag = targetDate
            };
            btnDate.FlatAppearance.BorderSize = 0;

            btnDate.Click += (s, e) =>
            {
                _selectedDate = (DateTime)((Button)s).Tag;
                RenderDateTabs();
                LoadShowtimes();
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

            Button btnTime = new Button
            {
                Text = isPast ? $"{st.StartTime:HH:mm}\n(Đã chiếu)" : $"{st.StartTime:HH:mm}\nPhòng {st.RoomId}\nCòn 140 ghế",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = isPast ? ColorTranslator.FromHtml("#64748B") : Color.White,
                BackColor = isPast ? ColorTranslator.FromHtml("#0F172A") : ColorTranslator.FromHtml("#181c20"),
                Size = new Size(160, 90),
                Margin = new Padding(0, 0, 20, 20),
                FlatStyle = FlatStyle.Flat,
                Enabled = !isPast, 
                Cursor = isPast ? Cursors.No : Cursors.Hand,
                Tag = st.ShowtimeId
            };
            btnTime.FlatAppearance.BorderColor = isPast ? ColorTranslator.FromHtml("#1E293B") : ColorTranslator.FromHtml("#40484f");

            if (!isPast)
            {
                btnTime.Click += (s, e) =>
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

                    lblSelectedShowtimeSummary.Text = $"SUẤT CHIẾU ĐANG CHỌN: IMAX LASER 4K • Phòng Chiếu {st.RoomId} • {st.StartTime:HH:mm}";
                    btnContinue.Enabled = true;
                    btnContinue.BackColor = ColorTranslator.FromHtml("#ffb961");
                };
            }

            pnlShowtimes.Controls.Add(btnTime);
        }
    }
}