using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Kiosk;

public class FrmKioskSeat : Form
{
    private int _showtimeId;
    private decimal _seatPrice = 160000m;
    private List<string> _selectedSeats = new List<string>();

    private Panel pnlHeader;
    private Panel pnlLeftInfo;
    private Panel pnlScreenArea;
    private FlowLayoutPanel pnlSeatGrid;
    private Panel pnlBottomBar;

    private Label lblClock;
    private Label lblMovieTitle;
    private Label lblShowtimeInfo;
    private Label lblSelectedSeats;
    private Label lblTotalPrice;
    private Button btnContinuePayment;
    private System.Windows.Forms.Timer _timer;

    public FrmKioskSeat(int showtimeId)
    {
        _showtimeId = showtimeId;
        InitializeComponent();
        LoadShowtimeDetails();
        RenderSeatMap();
    }
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.Text = "CineLuxe Kiosk - Chọn ghế";
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
            Location = new Point(550, 28),
            AutoSize = true
        };

        Label lblStep2 = new Label
        {
            Text = "2. Chọn Suất Chiếu",
            Font = new Font("Segoe UI", 11F),
            ForeColor = ColorTranslator.FromHtml("#8b9198"),
            Location = new Point(700, 28),
            AutoSize = true
        };
        Label lblStep3 = new Label
        {
            Text = "3. Chọn Ghế",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = ColorTranslator.FromHtml("#1a5276"),
            Padding = new Padding(16, 8, 16, 8),
            Location = new Point(870, 20),
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
        Button btnBack = new Button
        {
            Text = "← Quay lại",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#94A3B8"),
            BackColor = ColorTranslator.FromHtml("#262a2f"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(100, 38),
            Location = new Point(1660, 21),
            Cursor = Cursors.Hand
        };
        btnBack.FlatAppearance.BorderSize = 0;
        btnBack.Click += (s, e) =>
        {
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
        pnlHeader.Controls.Add(lblStep3);
        pnlHeader.Controls.Add(lblClock);
        pnlHeader.Controls.Add(btnBack);
        pnlHeader.Controls.Add(btnExit);
        pnlLeftInfo = new Panel
        {
            Size = new Size(380, 876),
            Location = new Point(24, 96),
            BackColor = ColorTranslator.FromHtml("#1c2024")
        };

        lblMovieTitle = new Label
        {
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"),
            Location = new Point(20, 20),
            AutoSize = true,
            MaximumSize = new Size(340, 0)
        };
        lblShowtimeInfo = new Label
        {
            Font = new Font("Segoe UI", 10F),
            ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
            Location = new Point(20, 80),
            AutoSize = true
        };
        Panel pnlLegend = new Panel
        {
            Location = new Point(20, 200),
            Size = new Size(340, 180),
            BackColor = ColorTranslator.FromHtml("#181c20")
        };

        Label lblLegendTitle = new Label
        {
            Text = "CHÚ THÍCH GHẾ",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#38bdf8"),
            Location = new Point(16, 16),
            AutoSize = true
        };
        pnlLegend.Controls.Add(lblLegendTitle);
        AddLegendItem(pnlLegend, "#181c20", "Ghế trống (Standard)", 50);
        AddLegendItem(pnlLegend, "#ffb961", "Ghế bạn đang chọn", 90);
        AddLegendItem(pnlLegend, "#334155", "Ghế đã có người đặt", 130);

        pnlLeftInfo.Controls.Add(lblMovieTitle);
        pnlLeftInfo.Controls.Add(lblShowtimeInfo);
        pnlLeftInfo.Controls.Add(pnlLegend);
        
        pnlScreenArea = new Panel
        {
            Size = new Size(1468, 876),
            Location = new Point(428, 96),
            BackColor = ColorTranslator.FromHtml("#101418")
        };
        Label lblScreen = new Label
        {
            Text = "M À N   H Ì N H   C H I Ế U   ( S C R E E N )",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#38bdf8"),
            BackColor = ColorTranslator.FromHtml("#1e293b"),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(1000, 40),
            Location = new Point(234, 20)
        };

        pnlSeatGrid = new FlowLayoutPanel
        {
            Size = new Size(1100, 750),
            Location = new Point(184, 90),
            AutoScroll = true,
            BackColor = ColorTranslator.FromHtml("#101418")
        };

        pnlScreenArea.Controls.Add(lblScreen);
        pnlScreenArea.Controls.Add(pnlSeatGrid);
        pnlBottomBar = new Panel
        {
            Size = new Size(1920, 84),
            Location = new Point(0, 988),
            BackColor = ColorTranslator.FromHtml("#0b0f12")
        };

        lblSelectedSeats = new Label
        {
            Text = "GHẾ ĐÃ CHỌN: (Chưa chọn ghế)",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
            Location = new Point(428, 30),
            AutoSize = true
        };

        lblTotalPrice = new Label
        {
            Text = "TỔNG TIỀN: 0 đ",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"),
            Location = new Point(1100, 26),
            AutoSize = true
        };
        btnContinuePayment = new Button
        {
            Text = "CHỌN BẮP NƯỚC →",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#101418"),
            BackColor = ColorTranslator.FromHtml("#282C31"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(220, 52),
            Location = new Point(1660, 16),
            Enabled = false,
            Cursor = Cursors.Hand
        };
        btnContinuePayment.FlatAppearance.BorderSize = 0;
        btnContinuePayment.Click += (s, e) =>
        {
            if (_selectedSeats.Any())
            {
                var holdService = new CinemaManagement.Services.SeatHoldService();
                bool success = holdService.HoldSeats(_showtimeId, _selectedSeats, holdDurationMinutes: 3);

                if (success)
                {
                    FrmKioskCombo frmCombo = new FrmKioskCombo(_showtimeId, _selectedSeats, _selectedSeats.Count * _seatPrice);
                    frmCombo.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Rất tiếc, một số ghế bạn chọn vừa bị giữ bởi khách hàng khác. Vui lòng chọn ghế khác!", "CineLuxe Kiosk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        };
        pnlBottomBar.Controls.Add(lblSelectedSeats);
        pnlBottomBar.Controls.Add(lblTotalPrice);
        pnlBottomBar.Controls.Add(btnContinuePayment);

        this.Controls.Add(pnlHeader);
        this.Controls.Add(pnlLeftInfo);
        this.Controls.Add(pnlScreenArea);
        this.Controls.Add(pnlBottomBar);
        this.ResumeLayout(false);
        _timer = new System.Windows.Forms.Timer { Interval = 1000 };
        _timer.Tick += (s, e) =>
        {
            if (lblClock != null && !lblClock.IsDisposed)
                lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        };
        _timer.Start();
    }
    private void AddLegendItem(Panel container, string hexColor, string text, int top)
    {
        Panel box = new Panel
        {
            Size = new Size(24, 24),
            Location = new Point(16, top),
            BackColor = ColorTranslator.FromHtml(hexColor)
        };
        Label lbl = new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
            Location = new Point(50, top + 2),
            AutoSize = true
        };
        container.Controls.Add(box);
        container.Controls.Add(lbl);
    }
    private void LoadShowtimeDetails()
    {
        using var db = new CinemaDbContext();
        var showtime = db.Showtimes.FirstOrDefault(s => s.ShowtimeId == _showtimeId);
        if (showtime != null)
        {
            var movie = db.Movies.FirstOrDefault(m => m.MovieId == showtime.MovieId);
            if (movie != null)
            {
                lblMovieTitle.Text = movie.Title.ToUpper();
                lblShowtimeInfo.Text = $"Suất: {showtime.StartTime:HH:mm - dd/MM/yyyy}\nPhòng chiếu: {showtime.RoomId}\nGiá vé: {_seatPrice:N0} đ/ghế";
            }
        }
    }
    private void RenderSeatMap()
    {
        pnlSeatGrid.Controls.Clear();
        string[] rows = { "A", "B", "C", "D", "E", "F", "G", "H" };
        int cols = 10;
        
        List<string> bookedSeats = new List<string> { "A3", "A4", "C5", "C6", "E1", "E2" };

        foreach (var row in rows)
        {
            for (int col = 1; col <= cols; col++)
            {
                string seatCode = $"{row}{col}";
                bool isBooked = bookedSeats.Contains(seatCode);

                Button btnSeat = new Button
                {
                    Text = seatCode,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Size = new Size(85, 65),
                    Margin = new Padding(8),
                    FlatStyle = FlatStyle.Flat,
                    Tag = seatCode
                };
                if (isBooked)
                {
                    btnSeat.BackColor = ColorTranslator.FromHtml("#334155");
                    btnSeat.ForeColor = ColorTranslator.FromHtml("#64748b");
                    btnSeat.Enabled = false;
                    btnSeat.FlatAppearance.BorderSize = 0;
                }
                else
                {
                    btnSeat.BackColor = ColorTranslator.FromHtml("#181c20");
                    btnSeat.ForeColor = Color.White;
                    btnSeat.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#334155");
                    btnSeat.Cursor = Cursors.Hand;

                    btnSeat.Click += (s, e) =>
                    {
                        ToggleSeatSelection(btnSeat, seatCode);
                    };
                }
                pnlSeatGrid.Controls.Add(btnSeat);
            }
            pnlSeatGrid.SetFlowBreak(pnlSeatGrid.Controls[pnlSeatGrid.Controls.Count - 1], true);
        }
    }

    private void ToggleSeatSelection(Button btnSeat, string seatCode)
    {
        if (_selectedSeats.Contains(seatCode))
        {
            _selectedSeats.Remove(seatCode);
            btnSeat.BackColor = ColorTranslator.FromHtml("#181c20");
            btnSeat.ForeColor = Color.White;
        }
        else
        {
            _selectedSeats.Add(seatCode);
            btnSeat.BackColor = ColorTranslator.FromHtml("#ffb961");
            btnSeat.ForeColor = ColorTranslator.FromHtml("#101418");
        }
        UpdateSummary();
    }
    private void UpdateSummary()
    {
        if (_selectedSeats.Any())
        {
            lblSelectedSeats.Text = $"GHẾ ĐÃ CHỌN ({_selectedSeats.Count}): {string.Join(", ", _selectedSeats)}";
            decimal total = _selectedSeats.Count * _seatPrice;
            lblTotalPrice.Text = $"TỔNG TIỀN: {total:N0} đ";
            btnContinuePayment.Enabled = true;
            btnContinuePayment.BackColor = ColorTranslator.FromHtml("#ffb961");
        }
        else
        {
            lblSelectedSeats.Text = "GHẾ ĐÃ CHỌN: (Chưa chọn ghế)";
            lblTotalPrice.Text = "TỔNG TIỀN: 0 đ";
            btnContinuePayment.Enabled = false;
            btnContinuePayment.BackColor = ColorTranslator.FromHtml("#282C31");
        }
    }
}