using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Kiosk
{
    public partial class FrmKioskHome : Form
    {
       private System.Windows.Forms.Timer _timer;

        public FrmKioskHome()
        {
            InitializeComponent();
            InitializeFilterButtons();
            RegisterEvents();
            LoadMovies();
        }

        private void InitializeFilterButtons()
        {
            pnlFilterBar.Controls.Clear();
            string[] filters = { "Tất cả phim", "Suất hôm nay", "Phim IMAX", "Rạp thường" };
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

                btnFilter.Click += (s, e) =>
                {
                    foreach (Control ctrl in pnlFilterBar.Controls)
                    {
                        if (ctrl is Button b)
                        {
                            b.BackColor = ColorTranslator.FromHtml("#262a2f");
                            b.ForeColor = ColorTranslator.FromHtml("#c1c7cf");
                        }
                    }
                    btnFilter.BackColor = ColorTranslator.FromHtml("#1a5276");
                    btnFilter.ForeColor = Color.White;

                    string filterType = "All";
                    if (f == "Suất hôm nay") filterType = "Today";
                    else if (f == "Phim IMAX") filterType = "IMAX";
                    else if (f == "Rạp thường") filterType = "Standard";

                    FilterMovies(filterType);
                };

                pnlFilterBar.Controls.Add(btnFilter);
                leftPos += 140;
            }
        }

        private void RegisterEvents()
        {
            btnExit.Click += (s, e) =>
            {
                _timer?.Stop();
                Form? mainForm = Application.OpenForms.Cast<Form>()
                    .FirstOrDefault(f => f.Name == "FrmMain" || f.Name == "FrmRoleSelection" || f.Tag?.ToString() == "Main");
                if (mainForm != null)
                {
                    mainForm.Show();
                }
                this.Close();
            };

            _timer = new System.Windows.Forms.Timer { Interval = 1000 };
            _timer.Tick += (s, e) =>
            {
                if (lblClock != null && !lblClock.IsDisposed)
                    lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _timer.Start();
        }

        private void FilterMovies(string filterType)
        {
            using var db = new CinemaDbContext();
            var today = DateTime.Today;

            IQueryable<Movie> query = db.Movies;

            switch (filterType)
            {
                case "Today":
                    query = db.Showtimes
                        .Where(st => st.StartTime.Date == today)
                        .Select(st => st.Movie!)
                        .Distinct();
                    break;

                case "IMAX":
                    query = db.Showtimes
                        .Include(st => st.Room)
                        .Where(st => st.Room != null && st.Room.RoomName.Contains("IMAX"))
                        .Select(st => st.Movie!)
                        .Distinct();
                    break;

                case "Standard":
                    query = db.Showtimes
                        .Include(st => st.Room)
                        .Where(st => st.Room == null || !st.Room.RoomName.Contains("IMAX"))
                        .Select(st => st.Movie!)
                        .Distinct();
                    break;

                case "All":
                default:
                    break;
            }

            var movieList = query.ToList();
            LoadMovies(movieList);
        }

        private void LoadMovies(List<Movie>? movies = null)
        {
            if (pnlMovieList == null)
                return;

            pnlMovieList.Controls.Clear();

            if (movies == null)
            {
                using var db = new CinemaDbContext();
                movies = db.Movies.ToList();
            }

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

                string relativePath = movie.PosterPath ?? "";
                if (!string.IsNullOrEmpty(relativePath))
                {
                    string cleanPath = relativePath.TrimStart('\\', '/');
                    string path1 = Path.Combine(Application.StartupPath, cleanPath);
                    string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", cleanPath);
                    string path3 = Path.Combine(Directory.GetCurrentDirectory(), cleanPath);

                    string finalPath = File.Exists(path1) ? path1 : (File.Exists(path2) ? path2 : (File.Exists(path3) ? path3 : ""));

                    if (!string.IsNullOrEmpty(finalPath))
                    {
                        try
                        {
                            using (var stream = new FileStream(finalPath, FileMode.Open, FileAccess.Read))
                            {
                                picPoster.Image = Image.FromStream(stream);
                            }
                        }
                        catch { }
                    }
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
            foreach (Form f in Application.OpenForms)
            {
                if (f != this && f.Visible)
                {
                    f.Hide();
                }
            }
            FrmKioskShowtime frmShowtime = new FrmKioskShowtime(movieId);
            frmShowtime.TopMost = true;
            frmShowtime.Show();
            frmShowtime.Activate();
            this.Hide();
        }
    }
}