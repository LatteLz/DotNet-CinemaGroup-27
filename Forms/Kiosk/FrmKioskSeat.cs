using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskSeat : Form
    {
        private readonly int _showtimeId;
        private int _movieId;
        private decimal _seatPrice = 160000m;
        private List<string> _selectedSeats = new();

        private Controls.SeatMapControl _seatMapControl = null!;
        private System.Windows.Forms.Timer _timer = null!;
        private int _remainingSeconds = 180;
        private System.Windows.Forms.Timer _countdownTimer = null!;

        public FrmKioskSeat(int showtimeId)
        {
            _showtimeId = showtimeId;
            InitializeComponent();
            InitializeLegendItems();
            RegisterEvents();
            LoadShowtimeDetails();
            InitSeatMapControl();
            StartCountdown();
        }

        private void InitializeLegendItems()
        {
            AddLegendItem(pnlLegend, "#222930", "Ghế trống (Standard)", 50);
            AddLegendItem(pnlLegend, "#ffb961", "Ghế bạn đang chọn", 90);
            AddLegendItem(pnlLegend, "#334155", "Ghế đã có người đặt", 130);
        }

        private void RegisterEvents()
        {
            btnBack.Click += (s, e) =>
            {
                _countdownTimer?.Stop();
                ReleaseSeatHolds();

                var showtimeForm = Application.OpenForms.OfType<FrmKioskShowtime>().FirstOrDefault();
                if (showtimeForm != null)
                {
                    showtimeForm.TopMost = true;
                    showtimeForm.Show();
                    showtimeForm.Activate();
                }
                else
                {
                    var newShowtime = new FrmKioskShowtime(_movieId);
                    newShowtime.TopMost = true;
                    newShowtime.Show();
                }
                this.Close();
            };

            btnExit.Click += (s, e) =>
            {
                _countdownTimer?.Stop();
                ReleaseSeatHolds();

                Form? mainForm = Application.OpenForms.Cast<Form>()
                    .FirstOrDefault(f => f.Name == "FrmMain" || f.Name == "FrmRoleSelection" || f.Tag?.ToString() == "Main");
                mainForm?.Show();
                this.Close();
            };

            btnContinuePayment.Click += (s, e) =>
            {
                if (_selectedSeats.Any())
                {
                    var holdService = new CinemaManagement.Services.SeatHoldService();
                    bool success = holdService.HoldSeats(_showtimeId, _selectedSeats, holdDurationMinutes: 3);

                    if (success)
                    {
                        FrmKioskCombo frmCombo = new FrmKioskCombo(_showtimeId, _selectedSeats, _selectedSeats.Count * _seatPrice);
                        frmCombo.TopMost = true;
                        frmCombo.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Rất tiếc, một số ghế bạn chọn vừa bị giữ bởi khách hàng khác. Vui lòng chọn ghế khác!",
                            "CineLuxe Kiosk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _seatMapControl?.LoadSeatMap(_showtimeId);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 ghế trước khi tiếp tục!",
                        "CineLuxe Kiosk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            _timer = new System.Windows.Forms.Timer { Interval = 1000 };
            _timer.Tick += (s, e) =>
            {
                if (lblClock != null && !lblClock.IsDisposed)
                    lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _timer.Start();

            this.FormClosed += (_, _) =>
            {
                _timer?.Dispose();
                _countdownTimer?.Dispose();
            };
        }

        private void InitSeatMapControl()
        {
            _seatMapControl = new Controls.SeatMapControl
            {
                Location = new Point(184, 90),
                Size = new Size(1100, 750)
            };
            _seatMapControl.OnSeatSelectionChanged += (selectedSeats) =>
            {
                _selectedSeats = selectedSeats;
                UpdateSummary();
            };

            pnlScreenArea.Controls.Add(_seatMapControl);
            _seatMapControl.LoadSeatMap(_showtimeId);
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
            var showtime = db.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .FirstOrDefault(s => s.ShowtimeId == _showtimeId);

            if (showtime != null)
            {
                _movieId = showtime.MovieId;
                if (showtime.Price > 0) _seatPrice = showtime.Price;

                string movieTitle = showtime.Movie?.Title ?? "PHIM CHIẾU RẠP";
                string roomName = showtime.Room?.RoomName ?? $"Phòng {showtime.RoomId}";

                lblMovieTitle.Text = movieTitle.ToUpper();
                lblShowtimeInfo.Text = $"Suất: {showtime.StartTime:HH:mm - dd/MM/yyyy}\nPhòng chiếu: {roomName}\nGiá vé: {_seatPrice:N0} đ/ghế";
            }
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

        private void StartCountdown()
        {
            _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _countdownTimer.Tick += (s, e) =>
            {
                _remainingSeconds--;

                if (_remainingSeconds >= 0)
                {
                    int mins = _remainingSeconds / 60;
                    int secs = _remainingSeconds % 60;
                    lblCountdown.Text = $"{mins:D2}:{secs:D2}";

                    if (_remainingSeconds <= 30)
                    {
                        lblCountdown.ForeColor = ColorTranslator.FromHtml("#ef4444");
                    }
                }
                else
                {
                    _countdownTimer.Stop();
                    ReleaseSeatHolds();

                    MessageBox.Show(
                        "Đã hết thời gian 3 phút giữ ghế! Hệ thống sẽ quay lại màn hình chọn phim.",
                        "Hết thời gian thao tác",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    new FrmKioskHome().Show();
                    this.Close();
                }
            };
            _countdownTimer.Start();
        }

        private void ReleaseSeatHolds()
        {
            try
            {
                using var db = new CinemaDbContext();
                var holds = db.SeatHolds.Where(h => h.ShowtimeId == _showtimeId).ToList();
                if (holds.Any())
                {
                    db.SeatHolds.RemoveRange(holds);
                    db.SaveChanges();
                }
            }
            catch { }
        }
    }