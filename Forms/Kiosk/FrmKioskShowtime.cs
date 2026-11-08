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
    private Button btnContinue;
    private int _selectedShowtimeId = 0;
    
    public FrmKioskShowtime (int movieId)
    {
        _movieId = movieId;
        InitializeComponent();
        LoadShowtimes();
    }
    private void InitializeComponent()
    {
        this.Text = "CineLuxe Kiosk - Chọn suất chiếu";
        this.Size = new Size(390,844);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = ColorTranslator.FromHtml("#101418");
        btnBack = new Button
        {
            Text = "← quay lại ",
            Font = new Font("Segoe UI",9.5F,FontStyle.Bold),
            ForeColor =  ColorTranslator.FromHtml("#94A3B8"),
            BackColor = ColorTranslator.FromHtml("#181C20"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(95,36),
            Location = new Point(12,12),
            Cursor = Cursors.Hand
        };
        btnBack.FlatAppearance.BorderSize = 0;
        btnBack.Click += (s, e) =>
        {
            FrmKioskHome home = new FrmKioskHome();
            home.Show();
            this.Close();
        };
        lblMovieTitle = new Label
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#F59E0B"), // Amber accent
            Location = new Point(120, 18),
            AutoSize = true,
            MaximumSize = new Size(250, 0)
        };
        pnlShowtimes = new FlowLayoutPanel
        {
            Location = new Point(12, 60),
            Size = new Size(366, 715),
            AutoScroll = true,
            Padding = new Padding(4),
            BackColor = ColorTranslator.FromHtml("#101418")
        };
        btnContinue = new Button
        {
            Text = "TIẾP TỤC CHỌN GHẾ →",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = ColorTranslator.FromHtml("#282C31"), // Xám mờ khi chưa chọn
            FlatStyle = FlatStyle.Flat,
            Height = 52,
            Dock = DockStyle.Bottom,
            Enabled = false,
            Cursor = Cursors.Hand
        };
        btnContinue.FlatAppearance.BorderSize = 0;
        btnContinue.Click += (s, e) =>
        {
        };
        Controls.Add(btnBack);
        Controls.Add(lblMovieTitle);
        Controls.Add(pnlShowtimes);
        Controls.Add(btnContinue);
    }
    private void LoadShowtimes()
    {
        using var db = new CinemaDbContext();
        var movies = db.Movies.FirstOrDefault(m => m.MovieId == _movieId);
        if (movies != null)
        {
            lblMovieTitle.Text = movies.Title.ToUpper();
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
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Margin = new Padding(10),
            };
            pnlShowtimes.Controls.Add(lblEmpty);
            return;
    }

        foreach (var st in showtimes)
        {
            Button btnTime = new Button
            {
                Text = $"{st.StartTime:HH:mm}\nPhòng {st.RoomId}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#22262B"),
                Size = new Size(110, 68),
                Margin = new Padding(4),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = st.ShowtimeId
            };
            btnTime.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#363C44");
            btnTime.Click += (s, e) =>
            {
                foreach (Control ctrl in pnlShowtimes.Controls)
                {
                    if (ctrl is Button b)
                    {
                        b.BackColor = ColorTranslator.FromHtml("#22262B");
                    }
                    
                }
                btnTime.BackColor = ColorTranslator.FromHtml("#0284C7");
                _selectedShowtimeId = st.ShowtimeId;
                btnContinue.Enabled = true;
                btnContinue.BackColor = ColorTranslator.FromHtml("#0284C7");
            };
            pnlShowtimes.Controls.Add(btnTime);
            }
        }
}