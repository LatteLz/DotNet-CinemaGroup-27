using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminDashboard : Form
    {
        private Panel pnlSidebar = null!;
        private Panel pnlHeader = null!;
        private Panel pnlContent = null!;
        private Panel pnlFooter = null!;

        private Label lblRevenueValue = null!;
        private Label lblRevenueSub = null!;
        private Label lblTicketsValue = null!;
        private Label lblTicketsSub = null!;
        private Label lblOccupancyValue = null!;
        private Label lblOccupancySub = null!;
        private Label lblShowtimesValue = null!;
        private Label lblShowtimesSub = null!;

        public FrmAdminDashboard()
        {
            InitializeComponent();
            SetupDashboardLayout();
            LoadRealtimeKpiData();
        }

        private void SetupDashboardLayout()
        {
            Width = 1280;
            Height = 720;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CineLuxe - Cinema Management System";
            BackColor = ColorTranslator.FromHtml("#101418");
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 256,
                BackColor = ColorTranslator.FromHtml("#0B0F12")
            };
            Controls.Add(pnlSidebar);

            AddSidebarMenuButton("Chọn vai trò", 50, "ROLE_SELECT", false);
            AddSidebarMenuButton("Bảng điều khiển", 100, "DASHBOARD", true);
            AddSidebarMenuButton("Quản lý danh mục phim", 150, "MOVIES", false);
            AddSidebarMenuButton("Chi tiết phim", 200, "MOVIE_DETAIL", false);
            AddSidebarMenuButton("Cấu hình phòng chiếu", 250, "ROOMS", false);
            AddSidebarMenuButton("Lịch chiếu phim", 300, "SHOWTIMES", false);

            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = ColorTranslator.FromHtml("#0B0F12")
            };
            Controls.Add(pnlHeader);

            Label lblTitle = new Label
            {
                Text = "BẢNG ĐIỀU HÀNH TỔNG QUAN (ADMIN DASHBOARD)",
                ForeColor = ColorTranslator.FromHtml("#CBD5E1"),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(20, 12),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);

            pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 32,
                BackColor = ColorTranslator.FromHtml("#0B0F12")
            };
            Controls.Add(pnlFooter);

            Label lblFooterStatus = new Label
            {
                Text = "PROJECTION ENGINE: ONLINE   |   Database: SQLite (cinema.db)",
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Location = new Point(15, 8),
                AutoSize = true
            };
            pnlFooter.Controls.Add(lblFooterStatus);

            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#101418"),
                AutoScroll = true
            };
            Controls.Add(pnlContent);

            CreateKpiCards();
        }

        private void AddSidebarMenuButton(string text, int topPosition, string menuKey, bool isActive)
        {
            Color backCol = isActive ? ColorTranslator.FromHtml("#0284C7") : ColorTranslator.FromHtml("#181C20");
            Button btn = new Button
            {
                Text = text,
                Tag = menuKey,
                Width = 220,
                Height = 40,
                Location = new Point(18, topPosition),
                FlatStyle = FlatStyle.Flat,
                ForeColor = ColorTranslator.FromHtml("#CBD5E1"),
                BackColor = backCol,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F, isActive ? FontStyle.Bold : FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            if (!isActive)
            {
                btn.MouseEnter += (_, _) => btn.BackColor = ColorTranslator.FromHtml("#282E35");
                btn.MouseLeave += (_, _) => btn.BackColor = backCol;
            }
            btn.Click += OnMenuButtonClick;

            pnlSidebar.Controls.Add(btn);
        }

        private void OnMenuButtonClick(object? sender, EventArgs e)
        {
            if (sender is Button { Tag: string menuKey })
            {
                try
                {
                    switch (menuKey)
                    {
                        case "ROLE_SELECT":
                            Close();
                            break;

                        case "DASHBOARD":
                            LoadRealtimeKpiData();
                            break;

                        case "MOVIES":
                            NavigateTo(new FrmAdminMovies());
                            break;

                        case "MOVIE_DETAIL":
                            MessageBox.Show("Vui lòng chọn một phim cụ thể từ danh mục phim để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "ROOMS":
                            NavigateTo(new FrmAdminRooms());
                            break;

                        case "SHOWTIMES":
                            NavigateTo(new FrmAdminShowtimes());
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi không thể mở màn hình:\n{ex.Message}\n\nChi tiết:\n{ex.InnerException?.Message}",
                        "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void NavigateTo(Form targetForm)
        {
            targetForm.StartPosition = FormStartPosition.CenterScreen;
            targetForm.FormClosed += (_, _) =>
            {
                if (!this.IsDisposed)
                {
                    this.Show();
                    LoadRealtimeKpiData();
                }
            };
            this.Hide();
            targetForm.Show();
        }

        private void CreateKpiCards()
        {
            Panel card1 = CreateMetricCard("DOANH THU HÔM NAY", out lblRevenueValue, out lblRevenueSub, 24, 20);
            Panel card2 = CreateMetricCard("SỐ VÉ ĐÃ BÁN HÔM NAY", out lblTicketsValue, out lblTicketsSub, 314, 20);
            Panel card3 = CreateMetricCard("TỶ LỆ LẤP ĐẦY RẠP", out lblOccupancyValue, out lblOccupancySub, 604, 20);
            Panel card4 = CreateMetricCard("SUẤT CHIẾU HÔM NAY", out lblShowtimesValue, out lblShowtimesSub, 894, 20);

            pnlContent.Controls.AddRange(new Control[] { card1, card2, card3, card4 });
        }

        private Panel CreateMetricCard(string title, out Label valLabel, out Label subLabel, int left, int top)
        {
            Panel panel = new Panel
            {
                Width = 266,
                Height = 110,
                Location = new Point(left, top),
                BackColor = ColorTranslator.FromHtml("#181C20")
            };

            Label lblTitle = new Label
            {
                Text = title,
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Location = new Point(16, 14),
                AutoSize = true
            };
            panel.Controls.Add(lblTitle);

            valLabel = new Label
            {
                Text = "0",
                ForeColor = ColorTranslator.FromHtml("#F39C12"),
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Location = new Point(14, 38),
                AutoSize = true
            };
            panel.Controls.Add(valLabel);

            subLabel = new Label
            {
                Text = "Đang tải dữ liệu...",
                ForeColor = ColorTranslator.FromHtml("#CBD5E1"),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                Location = new Point(16, 78),
                AutoSize = true
            };
            panel.Controls.Add(subLabel);

            return panel;
        }

        private void LoadRealtimeKpiData()
        {
            try
            {
                using var db = new CinemaDbContext();
                DateTime today = DateTime.Today;
                DateTime tomorrow = today.AddDays(1);

                decimal todayRevenue = db.Tickets
                    .Include(t => t.Booking)
                    .Where(t => t.Booking != null && t.Booking.CreatedAt >= today && t.Booking.CreatedAt < tomorrow)
                    .Sum(t => t.Price);

                lblRevenueValue.Text = todayRevenue > 0 ? $"{todayRevenue:N0} đ" : "0 đ";
                lblRevenueSub.Text = "Cập nhật từ hệ thống vé";

                int ticketsSoldToday = db.Tickets
                    .Include(t => t.Showtime)
                    .Count(t => t.Showtime != null && t.Showtime.StartTime >= today && t.Showtime.StartTime < tomorrow);

                int totalSeatsCapacityToday = db.Showtimes
                    .Include(s => s.Room)
                    .Where(s => s.StartTime >= today && s.StartTime < tomorrow && s.Room != null)
                    .Sum(s => s.Room.TotalSeats);

                lblTicketsValue.Text = totalSeatsCapacityToday > 0 
                    ? $"{ticketsSoldToday} / {totalSeatsCapacityToday} VÉ" 
                    : $"{ticketsSoldToday} VÉ";
                lblTicketsSub.Text = $"Tiến độ: {(totalSeatsCapacityToday > 0 ? ((double)ticketsSoldToday / totalSeatsCapacityToday * 100).ToString("F1") : "0")}%";

                double occupancyRate = totalSeatsCapacityToday > 0 
                    ? ((double)ticketsSoldToday / totalSeatsCapacityToday) * 100 
                    : 0;

                lblOccupancyValue.Text = $"{occupancyRate:F1}%";
                lblOccupancySub.Text = "Tính trên các suất chiếu hôm nay";

                int totalTodayShowtimes = db.Showtimes
                    .Count(s => s.StartTime >= today && s.StartTime < tomorrow);

                int completedShowtimes = db.Showtimes
                    .Count(s => s.StartTime >= today && s.StartTime < tomorrow && s.EndTime <= DateTime.Now);

                lblShowtimesValue.Text = $"{totalTodayShowtimes} SUẤT";
                lblShowtimesSub.Text = $"{completedShowtimes} suất đã hoàn thành";
            }
            catch
            {
                lblRevenueValue.Text = "0 đ";
                lblTicketsValue.Text = "0 VÉ";
                lblOccupancyValue.Text = "0.0%";
                lblShowtimesValue.Text = "0 SUẤT";
                lblRevenueSub.Text = lblTicketsSub.Text = lblOccupancySub.Text = lblShowtimesSub.Text = "CSDL sẵn sàng nhận dữ liệu";
            }
        }
    }
}