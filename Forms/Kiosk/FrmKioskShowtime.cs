using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;


namespace CinemaManagement.Forms.Kiosk;

public class FrmKioskShowtime : Form
{
     private int _movieId;
    private FlowLayoutPanel pnlShowtimes;
    private Label lblMovieTitle;
    private Button btnBack;
    
    public FrmKioskShowtime (int movieId)
    {
        _movieId = movieId;
        InitializeComponent();
        LoadShowtimes();
    }
    private void InitializeComponent()
    {
        this.Text = "CineLuxe Kiosk - Chọn suất chiếu";
        this.Size = new Size(500,900);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(18,24,38);
        btnBack = new Button
        {
            Text = "<- quay lại ",
            ForeColor =  Color.White,
            BackColor = Color.FromArgb(45,55,72),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(100,35),
            Location = new Point(15,15)
        };
        btnBack.Click += (s, e) =>
        {
            FrmKioskHome home = new FrmKioskHome();
            home.Show();
            this.Close();
        };
        lblMovieTitle = new Label
        {
            Font = new Font("Segoe UI",14F, FontStyle.Bold),
            ForeColor =  Color.White,
            Location = new Point(130,18),
            AutoSize = true,
        };
        Controls.Add(btnBack);
        Controls.Add(lblMovieTitle);
        Controls.Add(pnlShowtimes);
        
    }
    private void LoadShowtimes()
    {
        using var db = new CinemaDbContext();
        var movies = db.Movies.FirstOrDefault(m => m.MovieId == _movieId);
        if (movies != null)
        {
            lblMovieTitle.Text = movies.Title;
        }
        var showtimes = db.Showtimes
            .Where(s => s.MovieId == _movieId && s.StartTime >= DateTime.Now)
            .OrderBy(s => s.StartTime)
            .ToList();
        pnlShowtimes.Controls.Clear();
        if(!showtimes.Any())
        {
            Label lblEmpty = new Label
            {
                Text = "Hiện chưa có suất chiếu tiếp theo cho phim này",
                ForeColor =  Color.Gray,
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Margin =  new Padding(10),
            };
            pnlShowtimes.Controls.Add(lblEmpty);
            return;
    }

        foreach (var st in showtimes)
        {
            Button btnTime = new Button
            {
                Text = $"{st.StartTime:HH:mm}\nGiá: {st.TicketPrice:N0}đ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 41, 59),
                Size = new Size(130, 70),
                Margin = new Padding(8),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTime.Click += (s, e) =>
            {
                MessageBox.Show($"Đã chọn suất chiếu {st.StartTime:HH:mm}. Chuyển sang chọn ghế!", "Thông báo");
            };
            pnlShowtimes.Controls.Add(btnTime);
            }
        }
}