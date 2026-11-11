using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;
using System.IO;
namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskHome : Form

{
   private FlowLayoutPanel pnlMovieList;
    private Label lblHeader;
    private Panel pnlHeader;
    private Panel pnlFilterBar;
    private Label lblClock;
    private System.Windows.Forms.Timer _timer;

    public FrmKioskHome()
    {
        InitializeComponent();
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
            Location = new Point(600, 20),
            AutoSize = true
        };

        Label lblStep2 = new Label
        {
            Text = "2. Chọn Ghế",
            Font = new Font("Segoe UI", 11F, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#8b9198"),
            Location = new Point(780, 28),
            AutoSize = true
        };

        lblClock = new Label
        {
            Text = DateTime.Now.ToString("HH:mm:ss"),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"), 
            Location = new Point(1540, 26),
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
            Location = new Point(1670, 22),
            Cursor = Cursors.Hand
        };
        btnHelp.FlatAppearance.BorderSize = 0;
        Button btnExit = new Button
        {
            Text = "✕ THOÁT",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = ColorTranslator.FromHtml("#ef4444"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(100, 36),
            Location = new Point(1795, 22),
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

        pnlHeader.Controls.Add(lblHeader);
        pnlHeader.Controls.Add(lblStep1);
        pnlHeader.Controls.Add(lblStep2);
        pnlHeader.Controls.Add(lblClock);
        pnlHeader.Controls.Add(btnHelp);
        pnlHeader.Controls.Add(btnExit);
        
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
                
        pnlMovieList = new FlowLayoutPanel
    {
        Size = new Size(1872, 870), 
        Location = new Point(24, 150),
        AutoScroll = true,
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = true,
        Padding = new Padding(0),
        BackColor = ColorTranslator.FromHtml("#101418")
    };

    this.Controls.Add(pnlHeader);
    this.Controls.Add(pnlFilterBar);
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

        PictureBox picPoster = new PictureBox
        {
            Size = new Size(130, 190),
            Location = new Point(16, 16),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = ColorTranslator.FromHtml("#262a2f")
        };
        var posterProp = movie.GetType().GetProperty("PosterUrl") ?? movie.GetType().GetProperty("ImagePath");
        string posterPath = posterProp?.GetValue(movie)?.ToString();
        if (!string.IsNullOrEmpty(posterPath) && File.Exists(posterPath))
        {
            try { picPoster.Image = Image.FromFile(posterPath); } catch { }
        }

        Label lblTitle = new Label
        {
            Text = movie.Title.ToUpper(),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
            Location = new Point(160, 16),
            AutoSize = true,
            MaximumSize = new Size(270, 0)
        };
        
        Label lblGenre = new Label
        {
            Text = $"{movie.Genre}\n{movie.Duration} phút",
            Font = new Font("Segoe UI", 10F),
            ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
            Location = new Point(160, 70),
            AutoSize = true
        };
        var ageProp = movie.GetType().GetProperty("AgeRating") ?? movie.GetType().GetProperty("Rating");
        string ageRating = ageProp?.GetValue(movie)?.ToString() ?? "T16";
        Label lblRating = new Label
        {
            Text = $"{ageRating} • 8.8 ★",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"),
            Location = new Point(160, 125),
            AutoSize = true
        };

        Button btnBook = new Button
        {
            Text = "CHỌN SUẤT CHIẾU",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            BackColor = ColorTranslator.FromHtml("#38bdf8"),
            ForeColor = ColorTranslator.FromHtml("#101418"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(268, 44),
            Location = new Point(160, 162),
            Cursor = Cursors.Hand
        };
        btnBook.FlatAppearance.BorderSize = 0;

        int selectedMovieId = movie.MovieId;
        btnBook.Click += (s, e) => OpenShowtime(selectedMovieId);
        
        card.Controls.Add(picPoster);
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