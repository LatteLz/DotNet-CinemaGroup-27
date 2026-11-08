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

    public FrmKioskHome()
    {
        InitializeComponent();
        LoadMovies();
    }

    private void InitializeComponent()
    {
        this.Text = "CineLuxe Kiosk - Trang chủ";
        this.Size = new Size(390, 844);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = ColorTranslator.FromHtml("#101418");
        lblHeader = new Label()
        {
            Text = "Đang chiếu tại rạp",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = ColorTranslator.FromHtml("#181C20"),
            Dock = DockStyle.Top,
            Height = 60,
            TextAlign = ContentAlignment.MiddleCenter
        };
        pnlMovieList = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(12),
            BackColor = ColorTranslator.FromHtml("#101418")
        };
        this.Controls.Add(pnlMovieList);
        this.Controls.Add(lblHeader);
        pnlMovieList.BringToFront();
    }

    private void LoadMovies()
    {
        pnlMovieList.Controls.Clear();
        using var db = new CinemaDbContext();
        var movies = db.Movies.ToList();
        if (!movies.Any())
        {
            Label lblEmpty = new Label
            {
                Text = "Hiện chưa có dữ liệu phim.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                AutoSize = true,
                Margin = new Padding(10)
            };
            pnlMovieList.Controls.Add(lblEmpty);
            return;
        }

        foreach (var movie in movies)
        {
            Panel card = new Panel
            {
                Width = 342,
                Height = 125,
                BackColor = ColorTranslator.FromHtml("#181C20"),
                Margin = new Padding(12, 0, 12, 12),
            };
            Label lblTitle = new Label
            {
                Text = movie.Title.ToUpper(),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 10),
                AutoSize = true,
                MaximumSize = new Size(320, 0)
            };
            Label lblGenre = new Label
            {
                Text = $"{movie.Genre}  •  {movie.Duration} phút",
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorTranslator.FromHtml("#94A3B8"), 
                Location = new Point(12, 42),
                AutoSize = true
            };
            Button btnBook = new Button
            {
                Text = "ĐẶT VÉ NGAY",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#0284C7"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(322, 42),
                Location = new Point(12, 70),
                Cursor = Cursors.Hand
            };
            btnBook.FlatAppearance.BorderSize = 0;
            btnBook.Click += (s, e) =>
            {
                FrmKioskShowtime frmShowtime = new FrmKioskShowtime(movie.MovieId);
                frmShowtime.Show();
                this.Hide();
            };
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblGenre);
            card.Controls.Add(btnBook);
            pnlMovieList.Controls.Add(card);
        }
    }
}