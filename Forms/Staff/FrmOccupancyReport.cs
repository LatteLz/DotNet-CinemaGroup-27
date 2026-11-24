using System.Text;
using CinemaManagement.Forms.Shared;
using CinemaManagement.Helpers;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmOccupancyReport : Form
    {
        private readonly Color ColorTextWhite = Color.White;
        private readonly Color ColorTextMuted = ColorTranslator.FromHtml("#94a3b8");
        private readonly Color ColorCyan = ColorTranslator.FromHtml("#38bdf8");
        private readonly Color ColorAmber = ColorTranslator.FromHtml("#f59e0b");
        private readonly Color ColorRed = ColorTranslator.FromHtml("#ef4444");

        private readonly ReportService _reportService = new();
        private readonly RoomService _roomService = new();

        private ComboBox cmbRooms = null!;
        private ComboBox cmbHours = null!;
        private TrackBar trkMinRate = null!;
        private Label lblRateValue = null!;
        private DateTimePicker dtpReportDate = null!;
        private ErrorProvider errorProvider = new ErrorProvider();
        private readonly ToolTip _cellToolTip = new ToolTip();

        private List<ShowtimeOccupancyReportDto> _dayData = new();

        public FrmOccupancyReport()
        {
            InitializeComponent();
            AppSession.RequireAdmin(this);
            SetupSidebarEvents();
            SetupActionButtons();
            PopulateFilterBar();
            LoadRealDataFromDatabase();
            WindowLayout.ScaleToStandard(this);
        }

        private void SetupSidebarEvents()
        {
            if (lblUser != null) lblUser.Text = $"{AppSession.DisplayName}\r\n{AppSession.CurrentUser?.Role}";
            if (lnkPos != null) lnkPos.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmStaffPOS>(this);
            if (lnkCombo != null) lnkCombo.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminCombos>(this);
            if (lnkReports != null) lnkReports.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminReports>(this);
            if (lnkUsers != null) lnkUsers.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminUsers>(this);
        }

        private void SetupActionButtons()
        {
            if (btnToday != null)
            {
                btnToday.Click += (s, e) =>
                {
                    errorProvider.Clear();
                    dtpReportDate.Value = DateTime.Today;
                };
            }

            if (btnRefresh != null)
            {
                btnRefresh.Click += (s, e) =>
                {
                    errorProvider.Clear();
                    LoadRealDataFromDatabase();
                };
            }

            if (btnExport != null)
            {
                btnExport.Click += (s, e) =>
                {
                    var rows = GetFilteredRows();
                    if (rows.Count == 0)
                    {
                        errorProvider.SetError(btnExport, "Không có dữ liệu để xuất!");
                        MessageBox.Show("Không có suất chiếu nào phù hợp bộ lọc để xuất.", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    errorProvider.SetError(btnExport, string.Empty);

                    using var sfd = new SaveFileDialog
                    {
                        Filter = "CSV File (*.csv)|*.csv",
                        FileName = $"Occupancy_Report_{dtpReportDate.Value:yyyyMMdd}.csv"
                    };
                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    try
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("Ngay,Phong,Gio chieu,Phim,Ve da ban,Tong ghe,Ty le lap day,Danh gia");
                        foreach (var r in rows)
                        {
                            sb.AppendLine($"\"{r.StartTime:dd/MM/yyyy}\",\"{r.RoomName}\",\"{r.StartTime:HH:mm}\",\"{r.MovieName.Replace("\"", "\"\"")}\"," +
                                          $"{r.BookedSeats},{r.TotalSeats},{r.OccupancyRate:F1}%,\"{r.PerformanceStatus}\"");
                        }
                        File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                        MessageBox.Show("Xuất báo cáo lấp đầy thành công!\nĐường dẫn: " + sfd.FileName, "Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                    {
                        MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
            }
        }

        private void PopulateFilterBar()
        {
            if (pnlFilter == null) return;
            pnlFilter.Controls.Clear();

            AddLabel(pnlFilter, "🏢 PHÒNG CHIẾU", 15, 8, 8F, ColorTextMuted, true);
            cmbRooms = new ComboBox
            {
                Location = new Point(15, 26),
                Size = new Size(220, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ColorTranslator.FromHtml("#161a1e"),
                ForeColor = Color.White,
                DisplayMember = "Text",
                ValueMember = "Id"
            };
            var roomItems = new List<object> { new { Id = 0, Text = "Tất cả phòng" } };
            try
            {
                roomItems.AddRange(_roomService.GetAll().Select(r => (object)new { Id = r.RoomId, Text = r.RoomName }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách phòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmbRooms.DataSource = roomItems;
            cmbRooms.SelectedIndexChanged += (s, e) => RenderAll();
            pnlFilter.Controls.Add(cmbRooms);

            AddLabel(pnlFilter, "⏰ KHUNG GIỜ", 255, 8, 8F, ColorTextMuted, true);
            cmbHours = new ComboBox
            {
                Location = new Point(255, 26),
                Size = new Size(220, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ColorTranslator.FromHtml("#161a1e"),
                ForeColor = Color.White
            };
            cmbHours.Items.AddRange(new object[] { "Tất cả khung giờ", "Ca sáng (trước 12:00)", "Ca chiều (12:00 - 18:00)", "Ca tối (từ 18:00)" });
            cmbHours.SelectedIndex = 0;
            cmbHours.SelectedIndexChanged += (s, e) => RenderAll();
            pnlFilter.Controls.Add(cmbHours);

            AddLabel(pnlFilter, "📊 LẤP ĐẦY TỐI THIỂU", 495, 8, 8F, ColorTextMuted, true);
            trkMinRate = new TrackBar
            {
                Location = new Point(490, 24),
                Size = new Size(160, 30),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                TickStyle = TickStyle.None
            };
            lblRateValue = new Label
            {
                Text = "0%",
                Location = new Point(655, 28),
                Size = new Size(45, 20),
                ForeColor = ColorCyan,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            trkMinRate.Scroll += (s, e) =>
            {
                lblRateValue.Text = trkMinRate.Value + "%";
                RenderAll();
            };
            pnlFilter.Controls.Add(trkMinRate);
            pnlFilter.Controls.Add(lblRateValue);

            AddLabel(pnlFilter, "📅 NGÀY CHIẾU", 720, 8, 8F, ColorTextMuted, true);
            dtpReportDate = new DateTimePicker
            {
                Location = new Point(720, 26),
                Size = new Size(230, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            dtpReportDate.ValueChanged += (s, e) => LoadRealDataFromDatabase();
            pnlFilter.Controls.Add(dtpReportDate);
        }

        private void LoadRealDataFromDatabase()
        {
            DateTime day = dtpReportDate?.Value.Date ?? DateTime.Today;
            try
            {
                _dayData = _reportService.GetShowtimeOccupancy(day, day.AddDays(1));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn tỷ lệ lấp đầy: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _dayData = new List<ShowtimeOccupancyReportDto>();
            }
            RenderAll();
        }

        private List<ShowtimeOccupancyReportDto> GetFilteredRows()
        {
            int roomId = cmbRooms?.SelectedValue is int id ? id : 0;
            int shift = cmbHours?.SelectedIndex ?? 0;
            int minRate = trkMinRate?.Value ?? 0;

            return _dayData
                .Where(r => roomId == 0 || r.RoomId == roomId)
                .Where(r => shift switch
                {
                    1 => r.StartTime.Hour < 12,
                    2 => r.StartTime.Hour >= 12 && r.StartTime.Hour < 18,
                    3 => r.StartTime.Hour >= 18,
                    _ => true
                })
                .Where(r => r.OccupancyRate >= minRate)
                .OrderBy(r => r.RoomName).ThenBy(r => r.StartTime)
                .ToList();
        }

        private void RenderAll()
        {
            var rows = GetFilteredRows();
            RenderKpiCards(rows);
            RenderMatrix(rows);
            RenderAnalytics(rows);
        }

        private void RenderKpiCards(List<ShowtimeOccupancyReportDto> rows)
        {
            int sold = rows.Sum(r => r.BookedSeats);
            int seats = rows.Sum(r => r.TotalSeats);
            double avg = seats > 0 ? (double)sold / seats * 100 : 0;
            int hot = rows.Count(r => r.OccupancyRate >= ReportService.HotThreshold);
            int cold = rows.Count(r => r.OccupancyRate < ReportService.ColdThreshold);

            SetupCard(pnlCard1, "TỶ LỆ LẤP ĐẦY TRUNG BÌNH", $"{avg:F1}%", $"{rows.Count} suất trong bộ lọc", ColorCyan);
            SetupCard(pnlCard2, "VÉ ĐÃ BÁN / TỔNG GHẾ", $"{sold:N0} / {seats:N0}", "Theo các suất đang lọc");
            SetupCard(pnlCard3, "SUẤT CHÁY VÉ (≥ 80%)", $"{hot} suất", "Nên tăng suất chiếu", ColorAmber);
            SetupCard(pnlCard4, "SUẤT VẮNG (< 30%)", $"{cold} suất", "Cân nhắc giảm suất", ColorRed);
        }

        private void RenderMatrix(List<ShowtimeOccupancyReportDto> rows)
        {
            if (pnlMatrix == null) return;
            pnlMatrix.Controls.Clear();
            pnlMatrix.AutoScroll = true;

            AddLabel(pnlMatrix, $"Ma trận lấp đầy theo phòng và suất chiếu - {dtpReportDate.Value:dd/MM/yyyy}", 15, 12, 11F, ColorTextWhite, true);
            AddLabel(pnlMatrix, "Mỗi ô: giờ chiếu • tỷ lệ (vé đã bán/tổng ghế). Xanh ≥ 80%, vàng 30-79%, đỏ < 30%.", 15, 34, 8.5F, ColorTextMuted, false);

            if (rows.Count == 0)
            {
                AddLabel(pnlMatrix, "Không có suất chiếu nào phù hợp bộ lọc trong ngày này.", 15, 75, 9.5F, ColorAmber, true);
                return;
            }

            int startY = 62;
            foreach (var room in rows.GroupBy(r => r.RoomName))
            {
                AddLabel(pnlMatrix, room.Key, 12, startY + 8, 8.5F, ColorTextWhite, true);

                int cellX = 190;
                foreach (var r in room)
                {
                    var cell = new Button
                    {
                        Text = $"{r.StartTime:HH:mm} • {r.OccupancyRate:F0}%\n{r.BookedSeats}/{r.TotalSeats}",
                        Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                        Location = new Point(cellX, startY),
                        Size = new Size(105, 34),
                        FlatStyle = FlatStyle.Flat,
                        TabStop = false
                    };
                    cell.FlatAppearance.BorderSize = 1;

                    if (r.OccupancyRate >= ReportService.HotThreshold)
                    {
                        cell.BackColor = ColorTranslator.FromHtml("#0284c7");
                        cell.ForeColor = Color.White;
                        cell.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#38bdf8");
                    }
                    else if (r.OccupancyRate >= ReportService.ColdThreshold)
                    {
                        cell.BackColor = ColorTranslator.FromHtml("#854d0e");
                        cell.ForeColor = ColorTranslator.FromHtml("#fef08a");
                        cell.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eab308");
                    }
                    else
                    {
                        cell.BackColor = ColorTranslator.FromHtml("#7f1d1d");
                        cell.ForeColor = ColorTranslator.FromHtml("#fca5a5");
                        cell.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#b91c1c");
                    }

                    _cellToolTip.SetToolTip(cell, $"{r.MovieName}\n{r.StartTime:HH:mm} - {r.RoomName}\n{r.PerformanceStatus}");
                    pnlMatrix.Controls.Add(cell);
                    cellX += 112;
                }

                startY += 40;
            }
        }

        private void RenderAnalytics(List<ShowtimeOccupancyReportDto> rows)
        {
            if (pnlLeftAnalytics != null)
            {
                pnlLeftAnalytics.Controls.Clear();
                pnlLeftAnalytics.AutoScroll = true;
                AddLabel(pnlLeftAnalytics, "Xếp hạng phim theo tỷ lệ lấp đầy", 15, 12, 11F, ColorTextWhite, true);
                AddLabel(pnlLeftAnalytics, "Tổng vé đã bán ÷ tổng ghế của các suất trong bộ lọc", 15, 32, 8.5F, ColorTextMuted, false);

                var ranking = rows.GroupBy(r => r.MovieName)
                    .Select(g => new
                    {
                        Movie = g.Key,
                        Shows = g.Count(),
                        Sold = g.Sum(x => x.BookedSeats),
                        Seats = g.Sum(x => x.TotalSeats)
                    })
                    .Select(x => new { x.Movie, x.Shows, x.Sold, x.Seats, Rate = x.Seats > 0 ? (double)x.Sold / x.Seats * 100 : 0 })
                    .OrderByDescending(x => x.Rate)
                    .ToList();

                if (ranking.Count == 0)
                {
                    AddLabel(pnlLeftAnalytics, "Chưa có dữ liệu.", 15, 75, 9F, ColorAmber, true);
                }

                int y = 65;
                int rank = 1;
                foreach (var m in ranking)
                {
                    Color c = m.Rate >= ReportService.HotThreshold ? ColorCyan
                        : m.Rate < ReportService.ColdThreshold ? ColorRed : ColorTextWhite;
                    AddLabel(pnlLeftAnalytics,
                        $"{rank}. {m.Movie}  👉 {m.Rate:F1}%  ({m.Sold}/{m.Seats} ghế, {m.Shows} suất)  {ReportService.ClassifyOccupancy(m.Rate)}",
                        15, y, 9F, c, true);
                    y += 30;
                    rank++;
                }
            }

            if (pnlRightAnalytics != null)
            {
                pnlRightAnalytics.Controls.Clear();
                pnlRightAnalytics.AutoScroll = true;
                AddLabel(pnlRightAnalytics, "Cảnh báo suất vắng (< 30%)", 15, 12, 11F, ColorRed, true);

                var cold = rows.Where(r => r.OccupancyRate < ReportService.ColdThreshold && r.StartTime > DateTime.Now)
                    .OrderBy(r => r.StartTime)
                    .ToList();

                int y = 45;
                if (cold.Count == 0)
                {
                    AddLabel(pnlRightAnalytics, "Không có suất sắp chiếu nào bị vắng.", 15, y, 9F, ColorCyan, true);
                }
                foreach (var r in cold.Take(10))
                {
                    AddLabel(pnlRightAnalytics, $"⚠️ {r.StartTime:HH:mm} {r.RoomName}\n    {r.MovieName} - {r.OccupancyRate:F0}%", 15, y, 8.5F, ColorAmber, false);
                    y += 40;
                }
            }
        }

        private void SetupCard(Panel card, string title, string val, string sub, Color? valColor = null)
        {
            if (card == null) return;
            card.Controls.Clear();

            AddLabel(card, title, 12, 10, 7.5F, ColorTextMuted, true);
            AddLabel(card, val, 12, 30, 15F, valColor ?? ColorTextWhite, true);
            AddLabel(card, sub, 12, 72, 7.5F, ColorTextMuted, true);
        }

        private void AddLabel(Control parent, string text, int x, int y, float size, Color color, bool isBold)
        {
            Label lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", size, isBold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = color,
                Location = new Point(x, y),
                AutoSize = true
            };
            parent.Controls.Add(lbl);
        }
    }
}
