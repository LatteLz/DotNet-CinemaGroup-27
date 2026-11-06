using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Controls;

public class SeatMapControls : UserControl
{
    private FlowLayoutPanel pnlSeatMatrix;
    private readonly SeatHoldService _seatHoldService = new SeatHoldService();
    public event  EventHandler SeatSelectionChanged;
    public List<int> SelectedSeatIds {get; private set;} = new List<int>();
    public int CurrentShowtimeId{get; private set;}
    public SeatMapControls()
    {
        InitializeComponent();
        }
    private void InitializeComponent()
    {
        pnlSeatMatrix = new FlowLayoutPanel()
        {
            Dock = DockStyle.Fill,
            AutoScroll =  true,
            Padding = new Padding(10)   
        };
        Controls.Add(pnlSeatMatrix);
    }
    // load toàn bộ sơ đồ ghế của suất chiếu và cập nhật trạng thái
    public void LoadSeatmap(int showtimeId)
    {
        CurrentShowtimeId = showtimeId;
        SelectedSeatIds.Clear();
        pnlSeatMatrix.Controls.Clear();
        using var db = new CinemaDbContext();
        // lấy thông tin suất chiếu và danh sách ghế trong phòng
        var showtime = db.Showtimes.FirstOrDefault(s=>s.ShowtimeId == showtimeId);
        if (showtime == null) return;
        var seats = db.Seats.Where(s=> s.RoomId == showtime.RoomId).OrderBy(s => s.SeatNumber).ToList();
        // lấy danh sách ID các ghế đã bán và các ghế đang giữ 
        var soldSeatIds = db.Tickets.Where(t=> t.ShowtimeId == showtimeId).Select(t => t.SeatId).ToList();
        var heldSeatIds =_seatHoldService.GetHeldSeatIds(showtimeId);
        foreach (var seat in seats)
        {
           Button btnSeat = new Button 
           {
        Text = seat.SeatNumber, // dùng để hiển thị tên ghế như A1 , A2 ....
        Tag = seat.SeatId,
            Width =60,
                Height = 50,
                    Margin = new Padding(5),
                        Font = new Font("segoe UI",9F,FontStyle.Bold)
        };
           // phân loại trạng thái ghế để tô màu 
           if (soldSeatIds.Contains(seat.SeatId))
           {
               btnSeat.BackColor = Color.Gray; // trạng thái đã bán
               btnSeat.ForeColor = Color.White;
               btnSeat.Enabled = false;
           }
           else if (heldSeatIds.Contains(seat.SeatId))
           {
               btnSeat.BackColor = Color.DarkOrange; // đang đươc khách đặt trước
               btnSeat.ForeColor =  Color.White;
               btnSeat.Enabled = false;
           }
           else
           {
               btnSeat.BackColor = Color.DarkGreen; // ghế trống
               btnSeat.ForeColor =  Color.Black;
                   btnSeat.Click += BtnSeat_Click;
                   
           }
           pnlSeatMatrix.Controls.Add(btnSeat);
        }
    }
    private void BtnSeat_Click(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.Tag is int seatId)
        {
            if(SelectedSeatIds.Contains(seatId))
            {
                SelectedSeatIds.Remove(seatId);
                btn.BackColor = Color.DarkGreen; // trả về ghế trống
                btn.ForeColor = Color.Black;
            }
            else
                {
                SelectedSeatIds.Add(seatId);
                btn.BackColor = Color.Blue; // ghế khách đang chọm
                btn.ForeColor =  Color.White;
                }
            // báo cho form chủ biết danh sách ghế chọn vừa thay đổi
            SeatSelectionChanged?.Invoke(this, EventArgs.Empty);
}
}
}