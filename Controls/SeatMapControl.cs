using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Controls;

public class SeatMapControl : UserControl
{
    public static readonly Color ColorAvailable = ColorTranslator.FromHtml("#e2e8f0");
    public static readonly Color ColorVip = ColorTranslator.FromHtml("#c4b5fd");
    public static readonly Color ColorCouple = ColorTranslator.FromHtml("#f9a8d4");
    public static readonly Color ColorSelected = ColorTranslator.FromHtml("#facc15");
    public static readonly Color ColorSold = ColorTranslator.FromHtml("#475569");
    public static readonly Color ColorHeldByOther = ColorTranslator.FromHtml("#7f1d1d");

    private readonly SeatHoldService _seatHoldService = new();
    private readonly Dictionary<int, Button> _buttons = new();
    private readonly System.Windows.Forms.Timer _refreshTimer;
    private readonly ToolTip _toolTip = new();
    private SeatMapDto? _map;

    public int ShowtimeId { get; private set; }

    public string SessionId { get; private set; } = string.Empty;

    public event Action<List<string>>? OnSeatSelectionChanged;

    public IReadOnlyList<SeatStatusDto> SelectedSeats =>
        _map?.Seats.Where(s => s.State == SeatState.HeldByMe).ToList() ?? new List<SeatStatusDto>();

    public List<string> SelectedSeatNumbers => SelectedSeats.Select(s => s.SeatNumber).ToList();
    public List<int> SelectedSeatIds => SelectedSeats.Select(s => s.SeatId).ToList();
    public decimal SelectedTotal => SelectedSeats.Sum(s => s.Price);

    public SeatMapControl()
    {
        DoubleBuffered = true;
        BackColor = ColorTranslator.FromHtml("#101418");
        Size = new Size(900, 550);

        _refreshTimer = new System.Windows.Forms.Timer { Interval = 4000 };
        _refreshTimer.Tick += (_, _) => RefreshStates();
    }

    public void LoadSeatMap(int showtimeId, string sessionId)
    {
        ShowtimeId = showtimeId;
        SessionId = string.IsNullOrWhiteSpace(sessionId) ? SeatHoldService.NewSessionId() : sessionId;

        try
        {
            _seatHoldService.ReleaseExpiredHolds();
            _map = _seatHoldService.GetSeatMap(showtimeId, SessionId);
        }
        catch (Exception ex)
        {
            _map = null;
            Controls.Clear();
            Controls.Add(new Label
            {
                Text = "Không tải được sơ đồ ghế: " + ex.Message,
                ForeColor = Color.Salmon,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
            return;
        }

        RenderSeatGrid();
        _refreshTimer.Start();
        OnSeatSelectionChanged?.Invoke(SelectedSeatNumbers);
    }

    public void RefreshStates()
    {
        if (ShowtimeId <= 0 || IsDisposed) return;
        try
        {
            var latest = _seatHoldService.GetSeatMap(ShowtimeId, SessionId);
            bool layoutChanged = _map == null || latest.Seats.Count != _map.Seats.Count;
            var before = SelectedSeatNumbers;
            _map = latest;

            if (layoutChanged) RenderSeatGrid();
            else foreach (var seat in _map.Seats) ApplyStyle(seat);

            if (!before.SequenceEqual(SelectedSeatNumbers))
                OnSeatSelectionChanged?.Invoke(SelectedSeatNumbers);
        }
        catch
        {
        }
    }

    public void ReleaseAll()
    {
        try { _seatHoldService.ReleaseSession(SessionId); } catch { }
        if (ShowtimeId > 0) LoadSeatMap(ShowtimeId, SessionId);
    }

    public void StartNewSession()
    {
        if (ShowtimeId > 0) LoadSeatMap(ShowtimeId, SeatHoldService.NewSessionId());
    }

    private void RenderSeatGrid()
    {
        SuspendLayout();
        Controls.Clear();
        _buttons.Clear();

        if (_map == null || _map.Seats.Count == 0)
        {
            Controls.Add(new Label
            {
                Text = "Phòng chiếu chưa có sơ đồ ghế.",
                ForeColor = Color.Silver,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
            ResumeLayout();
            return;
        }

        int rows = Math.Max(_map.RowCount, _map.Seats.Max(s => s.RowIndex) + 1);
        int cols = Math.Max(_map.ColCount, _map.Seats.Max(s => s.ColIndex + s.ColumnSpan));

        var grid = new TableLayoutPanel
        {
            RowCount = rows,
            ColumnCount = cols,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
            BackColor = BackColor
        };
        for (int c = 0; c < cols; c++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
        for (int r = 0; r < rows; r++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));

        float fontSize = cols > 14 ? 7F : 8.5F;
        foreach (var seat in _map.Seats)
        {
            var btn = new Button
            {
                Text = seat.SeatNumber,
                Tag = seat.SeatId,
                Dock = DockStyle.Fill,
                Margin = new Padding(2),
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += SeatButton_Click;

            grid.Controls.Add(btn, seat.ColIndex, seat.RowIndex);
            if (seat.ColumnSpan > 1) grid.SetColumnSpan(btn, seat.ColumnSpan);

            _buttons[seat.SeatId] = btn;
            ApplyStyle(seat);
        }

        var screen = new Label
        {
            Text = "━━━━━━━━━━  MÀN HÌNH  ━━━━━━━━━━",
            Dock = DockStyle.Top,
            Height = 26,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = ColorTranslator.FromHtml("#94a3b8"),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold)
        };

        Controls.Add(grid);
        Controls.Add(screen);
        ResumeLayout();
    }

    private void ApplyStyle(SeatStatusDto seat)
    {
        if (!_buttons.TryGetValue(seat.SeatId, out var btn)) return;

        string typeLabel = SeatTypes.ToDisplay(seat.SeatType);
        Color baseColor = seat.SeatType switch
        {
            SeatTypes.Vip => ColorVip,
            SeatTypes.Couple => ColorCouple,
            _ => ColorAvailable
        };

        switch (seat.State)
        {
            case SeatState.Sold:
                btn.BackColor = ColorSold;
                btn.ForeColor = ColorTranslator.FromHtml("#94a3b8");
                btn.Enabled = false;
                btn.Cursor = Cursors.No;
                break;
            case SeatState.HeldByOther:
                btn.BackColor = ColorHeldByOther;
                btn.ForeColor = ColorTranslator.FromHtml("#fca5a5");
                btn.Enabled = false;
                btn.Cursor = Cursors.No;
                break;
            case SeatState.HeldByMe:
                btn.BackColor = ColorSelected;
                btn.ForeColor = ColorTranslator.FromHtml("#101418");
                btn.Enabled = true;
                btn.Cursor = Cursors.Hand;
                break;
            default:
                btn.BackColor = baseColor;
                btn.ForeColor = ColorTranslator.FromHtml("#101418");
                btn.Enabled = true;
                btn.Cursor = Cursors.Hand;
                break;
        }

        _toolTip.SetToolTip(btn, $"Ghế {seat.SeatNumber} ({typeLabel}) - {seat.Price:N0} đ");
    }

    private void SeatButton_Click(object? sender, EventArgs e)
    {
        if (_map == null || sender is not Button { Tag: int seatId }) return;
        var seat = _map.Seats.FirstOrDefault(s => s.SeatId == seatId);
        if (seat == null) return;

        try
        {
            if (seat.State == SeatState.HeldByMe)
            {
                _seatHoldService.ReleaseSeat(ShowtimeId, seatId, SessionId);
                seat.State = SeatState.Available;
            }
            else if (seat.State == SeatState.Available)
            {
                if (_seatHoldService.TryHoldSeat(ShowtimeId, seatId, SessionId, out string message))
                {
                    seat.State = SeatState.HeldByMe;
                }
                else
                {
                    MessageBox.Show(message, "Không thể chọn ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    RefreshStates();
                    return;
                }
            }

            ApplyStyle(seat);
            OnSeatSelectionChanged?.Invoke(SelectedSeatNumbers);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi giữ ghế: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            RefreshStates();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _refreshTimer.Stop();
            _refreshTimer.Dispose();
            _toolTip.Dispose();
        }
        base.Dispose(disposing);
    }
}
