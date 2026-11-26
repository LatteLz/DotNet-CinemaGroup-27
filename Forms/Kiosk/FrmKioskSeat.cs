using CinemaManagement.Controls;
using CinemaManagement.Forms.Shared;
using CinemaManagement.Models;
using CinemaManagement.Services;
using CinemaManagement.Helpers;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskSeat : Form
{
    private readonly int _showtimeId;
    private readonly string _sessionId = SeatHoldService.NewSessionId();
    private readonly ShowtimeService _showtimeService = new();
    private readonly SeatHoldService _seatHoldService = new();

    private int _movieId;
    private SeatMapControl _seatMapControl = null!;
    private System.Windows.Forms.Timer _clockTimer = null!;
    private System.Windows.Forms.Timer _countdownTimer = null!;
    private bool _expiredHandled;
    private readonly Dictionary<int, string> _lastAudiences = new();
    private FrmAudiencePicker? _activePicker;

    public string SessionId => _sessionId;

    public FrmKioskSeat(int showtimeId)
    {
        _showtimeId = showtimeId;
        InitializeComponent();
        InitializeLegendItems();
        RegisterEvents();
        LoadShowtimeDetails();
        InitSeatMapControl();
        StartCountdown();
        WindowLayout.ScaleToScreen(this);
    }

    private void InitializeLegendItems()
    {
        AddLegendItem(pnlLegend, SeatMapControl.ColorAvailable, "Ghế thường (trống)", 45);
        AddLegendItem(pnlLegend, SeatMapControl.ColorVip, "Ghế VIP (trống)", 71);
        AddLegendItem(pnlLegend, SeatMapControl.ColorCouple, "Ghế Sweetbox (trống)", 97);
        AddLegendItem(pnlLegend, SeatMapControl.ColorSelected, "Ghế bạn đang chọn", 123);
        AddLegendItem(pnlLegend, SeatMapControl.ColorSold, "Đã bán / đang có người giữ", 149);
    }

    private static void AddLegendItem(Control container, Color color, string text, int top)
    {
        container.Controls.Add(new Panel { Size = new Size(20, 20), Location = new Point(16, top), BackColor = color });
        container.Controls.Add(new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
            Location = new Point(46, top + 1),
            AutoSize = true
        });
    }

    private void RegisterEvents()
    {
        btnBack.Click += (_, _) =>
        {
            _countdownTimer.Stop();
            ReleaseMySeats();

            var showtimeForm = Application.OpenForms.OfType<FrmKioskShowtime>().FirstOrDefault();
            if (showtimeForm == null)
            {
                showtimeForm = new FrmKioskShowtime(_movieId) { TopMost = true };
            }
            showtimeForm.Show();
            showtimeForm.Activate();
            Close();
        };

        btnExit.Click += (_, _) =>
        {
            _countdownTimer.Stop();
            ReleaseMySeats();
            KioskFlow.ExitKiosk(this);
        };

        btnContinuePayment.Click += (_, _) =>
        {
            var seats = _seatMapControl.SelectedSeats;
            if (seats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế trước khi tiếp tục!", "CineLuxe Kiosk",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_seatHoldService.GetSessionExpiry(_sessionId) == null)
            {
                HandleExpired();
                return;
            }

            var showtime = _showtimeService.GetById(_showtimeId);
            if (showtime == null) return;

            using var picker = new FrmAudiencePicker(seats, showtime.Price, showtime.RoomType, showtime.StartTime, _lastAudiences) { TopMost = true };
            _activePicker = picker;
            DialogResult pickResult;
            try { pickResult = picker.ShowDialog(this); }
            finally { _activePicker = null; }
            if (pickResult != DialogResult.OK || _expiredHandled) return;
            if (_seatHoldService.GetSessionExpiry(_sessionId) == null)
            {
                HandleExpired();
                return;
            }

            foreach (var kv in picker.Selections) _lastAudiences[kv.Key] = kv.Value;
            var tickets = seats.Select(s => new KioskTicketLine
            {
                SeatId = s.SeatId,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType,
                AudienceType = picker.Selections.GetValueOrDefault(s.SeatId, AudienceTypes.Adult),
                Price = TicketPricing.TicketPrice(showtime.Price, showtime.RoomType, showtime.StartTime, s.SeatType,
                    picker.Selections.GetValueOrDefault(s.SeatId, AudienceTypes.Adult))
            }).ToList();

            var order = new KioskOrder
            {
                ShowtimeId = _showtimeId,
                MovieId = _movieId,
                SessionId = _sessionId,
                SeatIds = seats.Select(s => s.SeatId).ToList(),
                SeatNumbers = seats.Select(s => s.SeatNumber).ToList(),
                Tickets = tickets,
                TicketTotal = tickets.Sum(t => t.Price)
            };

            var frmCombo = new FrmKioskCombo(order) { TopMost = true };
            frmCombo.Show();
            Hide();
        };

        _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        _clockTimer.Tick += (_, _) =>
        {
            if (lblClock != null && !lblClock.IsDisposed)
                lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        };
        _clockTimer.Start();

        VisibleChanged += (_, _) =>
        {
            if (Visible && _seatMapControl != null) _seatMapControl.RefreshStates();
        };

        FormClosed += (_, _) =>
        {
            _clockTimer.Dispose();
            _countdownTimer.Dispose();
            ReleaseMySeats();
        };
    }

    private void LoadShowtimeDetails()
    {
        try
        {
            var showtime = _showtimeService.GetById(_showtimeId);
            if (showtime == null)
            {
                MessageBox.Show("Suất chiếu không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _movieId = showtime.MovieId;
            lblMovieTitle.Text = showtime.MovieTitle.ToUpper();
            lblShowtimeInfo.Text =
                $"Suất: {showtime.StartTime:HH:mm - dd/MM/yyyy}\nPhòng chiếu: {showtime.RoomName}\n" +
                $"Người lớn, ghế thường: {showtime.Price:N0} đ ({Models.DayTypes.ToDisplay(Models.DayTypes.Of(showtime.StartTime))}) • VIP +{Models.SeatTypes.VipSurcharge:N0} đ • Sweetbox (2 người): 2 × ({showtime.Price:N0} + {Models.SeatTypes.CoupleSurchargePerPerson:N0}) đ";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không tải được thông tin suất chiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void InitSeatMapControl()
    {
        _seatMapControl = new SeatMapControl
        {
            Location = new Point(184, 90),
            Size = new Size(1100, 750)
        };
        _seatMapControl.OnSeatSelectionChanged += _ => UpdateSummary();

        pnlScreenArea.Controls.Add(_seatMapControl);
        _seatMapControl.LoadSeatMap(_showtimeId, _sessionId);
    }

    private void UpdateSummary()
    {
        var seats = _seatMapControl.SelectedSeats;
        if (seats.Count > 0)
        {
            lblSelectedSeats.Text = $"GHẾ ĐÃ CHỌN ({seats.Count}): {string.Join(", ", seats.Select(s => s.SeatNumber))}";
            lblTotalPrice.Text = $"TẠM TÍNH: {seats.Sum(s => s.Price):N0} đ";
            btnContinuePayment.Enabled = true;
            btnContinuePayment.BackColor = ColorTranslator.FromHtml("#ffb961");
        }
        else
        {
            lblSelectedSeats.Text = "GHẾ ĐÃ CHỌN: (Chưa chọn ghế)";
            lblTotalPrice.Text = "TẠM TÍNH: 0 đ";
            btnContinuePayment.Enabled = false;
            btnContinuePayment.BackColor = ColorTranslator.FromHtml("#282C31");
        }
        UpdateCountdownLabel();
    }

    private void StartCountdown()
    {
        _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        _countdownTimer.Tick += (_, _) => UpdateCountdownLabel();
        _countdownTimer.Start();
        UpdateCountdownLabel();
    }

    private void UpdateCountdownLabel()
    {
        if (_expiredHandled || IsDisposed) return;

        DateTime? expiry;
        try { expiry = _seatHoldService.GetSessionExpiry(_sessionId); }
        catch { return; }

        bool hasSelection = _seatMapControl != null && _seatMapControl.SelectedSeats.Count > 0;
        if (expiry == null)
        {
            if (hasSelection)
            {
                HandleExpired();
                return;
            }
            lblCountdown.Text = "03:00";
            lblCountdown.ForeColor = ColorTranslator.FromHtml("#ffb961");
            return;
        }

        TimeSpan remaining = expiry.Value - DateTime.Now;
        if (remaining <= TimeSpan.Zero)
        {
            HandleExpired();
            return;
        }

        lblCountdown.Text = $"{(int)remaining.TotalMinutes:D2}:{remaining.Seconds:D2}";
        lblCountdown.ForeColor = remaining.TotalSeconds <= 30
            ? ColorTranslator.FromHtml("#ef4444")
            : ColorTranslator.FromHtml("#ffb961");
    }

    private void HandleExpired()
    {
        if (_expiredHandled) return;
        _expiredHandled = true;
        _countdownTimer?.Stop();

        if (_activePicker != null && !_activePicker.IsDisposed && _activePicker.Visible)
        {
            _activePicker.DialogResult = DialogResult.Cancel;
            BeginInvoke(new Action(() => KioskFlow.HandleHoldExpired(_sessionId, this)));
            return;
        }
        KioskFlow.HandleHoldExpired(_sessionId, this);
    }

    private void ReleaseMySeats()
    {
        try { _seatHoldService.ReleaseSession(_sessionId); }
        catch { }
    }
}
