using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskHome : Form

{
   private FlowLayoutPanel pnlMovieList;
    private Label lblHeader;
    private Panel pnlHeader;
    private Panel pnlFilterBar;
    private Panel pnlHeroSpotlight;
    private Label lblClock;
    private System.Windows.Forms.Timer _timer;

    public FrmKioskHome()
    {
        InitializeComponent();
        LoadHeroSection();
        LoadMovies();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.Text = "CineLuxe Kiosk - Trang chủ";
        this.Size = new Size(1920, 1080);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = ColorTranslator.FromHtml("#101418");

        pnlHeader = new Panel
        {
            Size = new Size(1920, 80),
            Location = new Point(0, 0),
            BackColor = ColorTranslator.FromHtml("#181c20")
        };

        lblHeader = new Label
        {
            Text = "CINELUXE KIOSK",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
            Location = new Point(24, 24),
            AutoSize = true
        };
        Label lblStep1 = new Label
        {
            Text = "1. Chọn Phim",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = ColorTranslator.FromHtml("#1a5276"),
            Padding = new Padding(16, 8, 16, 8),
            Location = new Point(650, 20),
            AutoSize = true
        };

        Label lblStep2 = new Label
        {
            Text = "2. Chọn Ghế",
            Font = new Font("Segoe UI", 11F, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#8b9198"),
            Location = new Point(800, 28),
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

        Button btnHelp = new Button
        {
            Text = "Trợ giúp",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"),
            BackColor = ColorTranslator.FromHtml("#6f4400"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(100, 36),
            Location = new Point(1780, 22),
            Cursor = Cursors.Hand
        };
        btnHelp.FlatAppearance.BorderSize = 0;

        pnlHeader.Controls.Add(lblHeader);
        pnlHeader.Controls.Add(lblStep1);
        pnlHeader.Controls.Add(lblStep2);
        pnlHeader.Controls.Add(lblClock);
        pnlHeader.Controls.Add(btnHelp);
        pnlFilterBar = new Panel
        {
            Size = new Size(1920, 56),
            Location = new Point(0, 80),
            BackColor = ColorTranslator.FromHtml("#101418")
        };

        string[] filters = { "Tất cả phim", "Suất hôm nay", "Phim IMAX", "Suất chiếu sớm" };
        int leftPos = 24;
        foreach (var f in filters)
        {
            Button btnFilter = new Button
            {
                Text = f,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = f == "Tất cả phim" ? Color.White : ColorTranslator.FromHtml("#c1c7cf"),
                BackColor = f == "Tất cả phim" ? ColorTranslator.FromHtml("#1a5276") : ColorTranslator.FromHtml("#262a2f"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 38),
                Location = new Point(leftPos, 8),
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;
            pnlFilterBar.Controls.Add(btnFilter);
            leftPos += 140;
        }
        pnlHeroSpotlight = new Panel
        {
            Size = new Size(1872, 340),
            Location = new Point(24, 146),
            BackColor = ColorTranslator.FromHtml("#1c2024")
        };
        pnlMovieList = new FlowLayoutPanel
        {
            Size = new Size(1872, 530),
            Location = new Point(24, 500),
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(0),
            BackColor = ColorTranslator.FromHtml("#101418")
        };

        this.Controls.Add(pnlHeader);
        this.Controls.Add(pnlFilterBar);
        this.Controls.Add(pnlHeroSpotlight);
        this.Controls.Add(pnlMovieList);
        this.ResumeLayout(false);
        
        _timer = new System.Windows.Forms.Timer { Interval = 1000 };
        _timer.Tick += (s, e) =>
        {
            if (lblClock != null && !lblClock.IsDisposed)
                lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        };
        _timer.Start();
    }

    private void LoadHeroSection()
    {
        if (pnlHeroSpotlight == null) return;
        pnlHeroSpotlight.Controls.Clear();

        Label lblTag = new Label
        {
            Text = "PHIM NỔI BẬT KI-ỐT  •  ĐỘC QUYỀN IMAX",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"),
            Location = new Point(32, 24),
            AutoSize = true
        };

        Label lblBadge = new Label
        {
            Text = "T18  •  ★ 9.4/10  •  166 phút  •  Khoa học viễn tưởng / Phiêu lưu",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
            Location = new Point(32, 54),
            AutoSize = true
        };

        Label lblTitle = new Label
        {
            Text = "DUNE: HÀNH TINH CÁT 2",
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
            Location = new Point(28, 84),
            AutoSize = true
        };

        Label lblDesc = new Label
        {
            Text = "Hành trình trả thù đầy cam go của Paul Atreides cùng người Fremen giữa tâm bão chính trị vũ trụ, mở ra định mệnh tối thượng trên cồn cát Arrakis.",
            Font = new Font("Segoe UI", 11F),
            ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
            Location = new Point(32, 140),
            Size = new Size(820, 50)
        };

        Button btnBookHero = new Button
        {
            Text = "⚡ ĐẶT VÉ NHANH",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#101418"),
            BackColor = ColorTranslator.FromHtml("#ffb961"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(190, 52),
            Location = new Point(32, 220),
            Cursor = Cursors.Hand
        };
        btnBookHero.FlatAppearance.BorderSize = 0;
        btnBookHero.Click += (s, e) => OpenShowtime(1);
        {
        };

        Button btnTrailer = new Button
        {
            Text = "▶ Xem Trailer (2m14s)",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
            BackColor = ColorTranslator.FromHtml("#262a2f"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(180, 52),
            Location = new Point(236, 220),
            Cursor = Cursors.Hand
        };
        btnTrailer.FlatAppearance.BorderSize = 0;

        pnlHeroSpotlight.Controls.Add(lblTag);
        pnlHeroSpotlight.Controls.Add(lblBadge);
        pnlHeroSpotlight.Controls.Add(lblTitle);
        pnlHeroSpotlight.Controls.Add(lblDesc);
        pnlHeroSpotlight.Controls.Add(btnBookHero);
        pnlHeroSpotlight.Controls.Add(btnTrailer);
    }

    private void LoadMovies()
    {
        if (pnlMovieList == null)
            return;
        pnlMovieList.Controls.Clear();
        using var db = new CinemaDbContext();
        var movies = db.Movies.ToList();

        if (!movies.Any())
        {
            Label lblEmpty = new Label
            {
                Text = "Hiện chưa có dữ liệu phim.",
                Font = new Font("Segoe UI", 12F),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                AutoSize = true,
                Margin = new Padding(20)
            };
            pnlMovieList.Controls.Add(lblEmpty);
            return;
        }

        foreach (var movie in movies)
        {
            Panel card = new Panel
            {
                Width = 444,
                Height = 220,
                BackColor = ColorTranslator.FromHtml("#1c2024"),
                Margin = new Padding(0, 0, 24, 24)
            };

            Label lblTitle = new Label
            {
                Text = movie.Title.ToUpper(),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(16, 16),
                AutoSize = true,
                MaximumSize = new Size(410, 0)
            };

            Label lblGenre = new Label
            {
                Text = $"{movie.Genre} • {movie.Duration} phút",
                Font = new Font("Segoe UI", 10F),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                Location = new Point(16, 50),
                AutoSize = true
            };
            Label lblRating = new Label
            {
                Text = "T16 • 8.8 ★ IMDb",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(16, 76),
                AutoSize = true
            };

            Button btnBook = new Button
            {
                Text = "Chọn suất chiếu",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#38bdf8"),
                ForeColor = ColorTranslator.FromHtml("#101418"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(412, 46),
                Location = new Point(16,150),
                Cursor = Cursors.Hand
            };
            btnBook.FlatAppearance.BorderSize = 0;
            int selectedMovieId = movie.MovieId;
            btnBook.Click += (s, e) => OpenShowtime(selectedMovieId);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblGenre);
            card.Controls.Add(lblRating);
            card.Controls.Add(btnBook);
            pnlMovieList.Controls.Add(card);
        }
    } 
    private void OpenShowtime(int movieId)
    {
        FrmKioskShowtime frmShowtime = new FrmKioskShowtime(movieId);
        frmShowtime.Show();
        this.Hide();
    }
}