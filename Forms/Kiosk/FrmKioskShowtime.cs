using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;
using Timer = System.Windows.Forms.Timer;

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
    
    public FrmKioskShowtime (int movieId)
    {
        _movieId = movieId;
        InitializeComponent();
        LoadShowtimes();
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
            Location = new Point(650, 28),
            AutoSize = true
        };

        Label lblStep2 = new Label
        {
            Text = "2. Chọn Suất Chiếu",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = ColorTranslator.FromHtml("#1a5276"),
            Padding = new Padding(16, 8, 16, 8),
            Location = new Point(780, 20),
            AutoSize = true
        };

        lblClock = new Label
        {
            Text = DateTime.Now.ToString("HH:mm:ss"),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"),
            Location = new Point(1650, 26),
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
            Location = new Point(1780,21),
            Cursor = Cursors.Hand
        };
        btnBack.FlatAppearance.BorderSize = 0;
        btnBack.Click += (s, e) =>
        {
            FrmKioskHome home = new FrmKioskHome();
            home.Show();
            this.Close();
        };
        pnlHeader.Controls.Add(lblLogo);
        pnlHeader.Controls.Add(lblStep1);
        pnlHeader.Controls.Add(lblStep2);
        pnlHeader.Controls.Add(lblClock);
        pnlHeader.Controls.Add(btnBack);
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

        string[] dates = { "HÔM NAY 26", "NGÀY MAI 27", "THỨ HAI 28", "THỨ BA 29" };
        for (int i = 0; i < dates.Length; i++)
        {
            Button btnDate = new Button
            {
                Text = dates[i],
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = i == 0 ? ColorTranslator.FromHtml("#101418") : ColorTranslator.FromHtml("#c1c7cf"),
                BackColor = i == 0 ? ColorTranslator.FromHtml("#38bdf8") : ColorTranslator.FromHtml("#181c20"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 60),
                Margin = new Padding(0, 0, 16, 0),
                Cursor = Cursors.Hand
            };
            btnDate.FlatAppearance.BorderSize = 0;
            pnlDates.Controls.Add(btnDate);
        }
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
           // tạm thời chưa tạo FrmKioskSeat
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
    private void LoadShowtimes()
    {
        using var db = new CinemaDbContext();
        var movies = db.Movies.FirstOrDefault(m => m.MovieId == _movieId);
        if (movies != null)
        {
            lblMovieTitle.Text = movies.Title.ToUpper();
            Label lblBadgeAge = new Label
            {
                Text = "T16 • CẤM DƯỚI 16 TUỔI",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#ef4444"),
                Padding = new Padding(8, 4, 8, 4),
                Location = new Point(24, 74),
                AutoSize = true
            };
           
            Label lblDetail = new Label
            {
                Text = $"Thể loại: {movies.Genre}\nThời lượng: {movies.Duration} phút\nCông nghệ: IMAX LASER 4K\nĐánh giá: 8.9/10 ★",
                Font = new Font("Segoe UI", 10F),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                Location = new Point(24, 120),
                AutoSize = true
            };

            Label lblDescHeader = new Label
            {
                Text = "TÓM TẮT NỘI DUNG",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(24, 240),
                AutoSize = true
            };

            Label lblDesc = new Label
            {
                Text = "Paul Atreides tái hợp cùng Chani và tộc người Fremen khi anh tìm kiếm sự báo thù chống lại những kẻ đã hủy diệt gia tộc mình. Đứng trước sự lựa chọn giữa tình yêu và số phận vũ trụ...",
                Font = new Font("Segoe UI", 10F),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                Location = new Point(24, 270),
                Size = new Size(350, 160)
            };

            pnlLeftMovieInfo.Controls.Add(lblBadgeAge);
            pnlLeftMovieInfo.Controls.Add(lblDetail);
            pnlLeftMovieInfo.Controls.Add(lblDescHeader);
            pnlLeftMovieInfo.Controls.Add(lblDesc);
        }
        var showtimes = db.Showtimes
            .Where(s => s.MovieId == _movieId)
            .OrderBy(s => s.StartTime)
            .ToList();
        pnlShowtimes.Controls.Clear();
        if(!showtimes.Any())
        {
            Label lblEmpty = new Label
            {
                Text = "Hiện chưa có suất chiếu tiếp theo cho phim này.",
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

        foreach (var st in showtimes)
        {
            Button btnTime = new Button
            {
                Text = $"{st.StartTime:HH:mm}\nPhòng {st.RoomId}\n Còn 140 ghế",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#181c20"),
                Size = new Size(160, 90),
                Margin = new Padding(0,0,20,20),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = st.ShowtimeId
            };
            btnTime.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#40484f");
            btnTime.Click += (s, e) =>
            {
                foreach (Control ctrl in pnlShowtimes.Controls)
                {
                    if (ctrl is Button b)
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
            pnlShowtimes.Controls.Add(btnTime);
            }
        }
}