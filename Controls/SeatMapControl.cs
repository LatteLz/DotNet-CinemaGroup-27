using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Services;

namespace CinemaManagement.Controls;
public class SeatMapControl : UserControl
{
    private readonly SeatHoldService _seatHoldService = new();
    private int _showtimeId;
    private readonly List<string> _selectedSeats = new();
    private readonly List<string> _bookedSeats = new();
    private readonly List<string> _heldSeats = new();

    public event Action<List<string>>? OnSeatSelectionChanged;

    public List<string> SelectedSeats => _selectedSeats;

    public SeatMapControl()
    {
        this.DoubleBuffered = true;
        this.BackColor = ColorTranslator.FromHtml("#101418");
        this.Size = new Size(900, 550);
    }
    public void LoadSeatMap(int showtimeId)
    {
        _showtimeId = showtimeId;
        _selectedSeats.Clear();
        _seatHoldService.ReleaseExpiredHolds();

        FetchBookedAndHeldSeats();
        RenderSeatGrid();
    }
    private void FetchBookedAndHeldSeats()
    {
        _bookedSeats.Clear();
        _heldSeats.Clear();
        try
        {
            using var db = new CinemaDbContext();
            var tickets = db.Tickets.Where(t => t.ShowtimeId == _showtimeId).ToList();
            foreach (var t in tickets)
            {
                var seat = db.Seats.FirstOrDefault(s => s.SeatId == t.SeatId);
                if (seat != null && !string.IsNullOrEmpty(seat.SeatNumber))
                    _bookedSeats.Add(seat.SeatNumber);
            }
            var holds = db.SeatHolds.Where(h => h.ShowtimeId == _showtimeId && h.ExpiresAt > DateTime.Now).ToList();
            foreach (var h in holds)
            {
                var seat = db.Seats.FirstOrDefault(s => s.SeatId == h.SeatId);
                if (seat != null && !string.IsNullOrEmpty(seat.SeatNumber))
                    _heldSeats.Add(seat.SeatNumber);
            }
        }
        catch { }
    }
    private void RenderSeatGrid()
    {
        this.Controls.Clear();
        TableLayoutPanel grid = new TableLayoutPanel
        {
            RowCount = 8,
            ColumnCount = 10,
            Dock = DockStyle.Fill,
            Padding = new Padding(10)
        };

        for (int i = 0; i < 10; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
        for (int i = 0; i < 8; i++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));

        char[] rows = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

        for (int r = 0; r < 8; r++)
        {
            for (int c = 1; c <= 10; c++)
            {
                string seatCode = $"{rows[r]}{c}";
                Button btnSeat = new Button
                {
                    Text = seatCode,
                    Tag = seatCode,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(4),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnSeat.FlatAppearance.BorderSize = 0;
                if (_bookedSeats.Contains(seatCode))
                {
                    btnSeat.BackColor = ColorTranslator.FromHtml("#475569");
                    btnSeat.ForeColor = ColorTranslator.FromHtml("#94A3B8");
                    btnSeat.Enabled = false;
                }
                else if (_heldSeats.Contains(seatCode))
                {
                    btnSeat.BackColor = ColorTranslator.FromHtml("#ef4444");
                    btnSeat.ForeColor = Color.White;
                    btnSeat.Enabled = false;
                }
                else
                {
                    btnSeat.BackColor = ColorTranslator.FromHtml("#1e293b");
                    btnSeat.ForeColor = Color.White;
                    btnSeat.Click += SeatButton_Click;
                }

                grid.Controls.Add(btnSeat, c - 1, r);
            }
        }

        this.Controls.Add(grid);
    }
    private void SeatButton_Click(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.Tag is string seatCode)
        {
            if (_selectedSeats.Contains(seatCode))
            {
                _selectedSeats.Remove(seatCode);
                btn.BackColor = ColorTranslator.FromHtml("#1e293b");
                btn.ForeColor = Color.White;
            }
            else
            {
                _selectedSeats.Add(seatCode);
                btn.BackColor = ColorTranslator.FromHtml("#ffb961");
                btn.ForeColor = ColorTranslator.FromHtml("#101418");
            }

            OnSeatSelectionChanged?.Invoke(_selectedSeats);
        }
    }
    }

    