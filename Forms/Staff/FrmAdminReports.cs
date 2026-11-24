using System.Drawing.Drawing2D;
using CinemaManagement.Forms.Shared;
using CinemaManagement.Helpers;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmAdminReports : Form
    {
        private readonly Color ColorBg = ColorTranslator.FromHtml("#0b0f12");
        private readonly Color ColorSurface = ColorTranslator.FromHtml("#101418");
        private readonly Color ColorCardBg = ColorTranslator.FromHtml("#181c20");
        private readonly Color ColorPrimaryCyan = ColorTranslator.FromHtml("#0284c7");
        private readonly Color ColorAccentAmber = ColorTranslator.FromHtml("#f59e0b");
        private readonly Color ColorTextMuted = ColorTranslator.FromHtml("#94a3b8");

        private readonly ReportService _reportService = new();
        private Panel[] kpiCards = new Panel[4];
        private ErrorProvider errorProvider = new ErrorProvider();

        private enum Period { Today, ThisMonth, AllTime }
        private Period _period = Period.ThisMonth;
        private string _periodLabel = "";

        private RevenueSummaryDto _summary = new();
        private List<MovieRevenueReportDto> _byMovie = new();

        public FrmAdminReports()
        {
            InitializeComponent();
            AppSession.RequireAdmin(this);
            InitializeCustomEvents();
            LoadRealDataFromDatabase();
            WindowLayout.ScaleToStandard(this);
        }

        private (DateTime From, DateTime To) GetRange()
        {
            DateTime today = DateTime.Today;
            return _period switch
            {
                Period.Today => (today, today.AddDays(1)),
                Period.ThisMonth => (new DateTime(today.Year, today.Month, 1), new DateTime(today.Year, today.Month, 1).AddMonths(1)),
                _ => (DateTime.MinValue, DateTime.MaxValue)
            };
        }

        private void InitializeCustomEvents()
        {
            this.BackColor = ColorBg;
            this.ForeColor = Color.White;

            if (lblUser != null) lblUser.Text = $"{AppSession.DisplayName}\r\n{AppSession.CurrentUser?.Role}";
            if (lblShift != null) lblShift.Text = $"NGÀY: {DateTime.Now:dd/MM/yyyy}";

            if (lnkPos != null) lnkPos.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmStaffPOS>(this);
            if (lnkCombo != null) lnkCombo.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminCombos>(this);
            if (lnkOccupancy != null) lnkOccupancy.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmOccupancyReport>(this);
            if (lnkUsers != null) lnkUsers.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminUsers>(this);

            if (btnMonthThis != null)
            {
                btnMonthThis.Click += (s, e) =>
                {
                    _period = _period switch
                    {
                        Period.Today => Period.ThisMonth,
                        Period.ThisMonth => Period.AllTime,
                        _ => Period.Today
                    };
                    LoadRealDataFromDatabase();
                };
            }

            if (btnExportExcel != null)
            {
                btnExportExcel.Click += (s, e) => {
                    if (dgvReports.Rows.Count == 0)
                    {
                        errorProvider.SetError(btnExportExcel, "Không có dữ liệu để xuất file báo cáo!");
                        MessageBox.Show("Không có dữ liệu báo cáo để xuất file.", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    errorProvider.SetError(btnExportExcel, string.Empty);

                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*";
                        sfd.FileName = "Cineluxe_Financial_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                                {
                                    sw.WriteLine($"BAO CAO DOANH THU THEO PHIM - {_periodLabel}");
                                    sw.WriteLine("STT,TEN PHIM,DOANH THU VE,SO SUAT,TONG VE,LAP DAY TB,TONG DOANH THU (VND),TY TRONG");
                                    foreach (DataGridViewRow row in dgvReports.Rows)
                                    {
                                        if (row.IsNewRow) continue;
                                        string stt = row.Cells[0].Value?.ToString() ?? "";
                                        string name = row.Cells[1].Value?.ToString() ?? "";
                                        string cat = row.Cells[2].Value?.ToString() ?? "";
                                        string showtime = row.Cells[3].Value?.ToString() ?? "";
                                        string tickets = row.Cells[4].Value?.ToString() ?? "";
                                        string occupancy = row.Cells[5].Value?.ToString() ?? "";
                                        string revenue = row.Cells[6].Value?.ToString() ?? "";
                                        string time = row.Cells[7].Value?.ToString() ?? "";

                                        sw.WriteLine($"\"{stt}\",\"{name}\",\"{cat}\",\"{showtime}\",\"{tickets}\",\"{occupancy}\",\"{revenue}\",\"{time}\"");
                                    }
                                }
                                MessageBox.Show("Xuất file báo cáo tài chính thành công!\nĐường dẫn: " + sfd.FileName, "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                };
            }

            BuildKpiCardsDynamic();
            StyleDataGridView();

            pnlBarChart.Paint += PnlBarChart_Paint;
            pnlDonutChart.Paint += PnlDonutChart_Paint;
        }

        private void BuildKpiCardsDynamic()
        {
            string[] titles = {
                "TỔNG DOANH THU KỲ NÀY",
                "DOANH THU VÉ PHIM",
                "DOANH THU F&B BẮP NƯỚC",
                "GIAO DỊCH ĐÃ CHỐT"
            };

            for (int i = 0; i < 4; i++)
            {
                var card = new Panel
                {
                    Size = new Size(283, 95),
                    Margin = new Padding(0, 0, 15, 0),
                    BackColor = ColorCardBg
                };

                var lblTitle = new Label
                {
                    Text = titles[i],
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = ColorTextMuted,
                    Location = new Point(12, 12),
                    Size = new Size(250, 18)
                };
                card.Controls.Add(lblTitle);

                var lblVal = new Label
                {
                    Name = $"lblKpiVal{i}",
                    Text = "0 đ",
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(10, 32),
                    Size = new Size(260, 35)
                };
                card.Controls.Add(lblVal);

                var lblSub = new Label
                {
                    Name = $"lblKpiSub{i}",
                    Text = "Đang đồng bộ từ CSDL...",
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = ColorTranslator.FromHtml("#4ade80"),
                    Location = new Point(12, 70),
                    Size = new Size(250, 18)
                };
                card.Controls.Add(lblSub);

                kpiCards[i] = card;
                pnlKpiContainer.Controls.Add(card);
            }
        }

        private void StyleDataGridView()
        {
            dgvReports.BackgroundColor = ColorSurface;
            dgvReports.ForeColor = Color.White;
            dgvReports.GridColor = ColorTranslator.FromHtml("#1f2937");
            dgvReports.DefaultCellStyle.BackColor = ColorSurface;
            dgvReports.DefaultCellStyle.ForeColor = Color.White;
            dgvReports.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1e242a");
            dgvReports.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = ColorCardBg;
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextMuted;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.RowTemplate.Height = 35;

            ColSTT.Width = 40;
            ColName.Width = 280;
        }

        private void LoadRealDataFromDatabase()
        {
            var (from, to) = GetRange();
            _periodLabel = _period switch
            {
                Period.Today => $"Hôm nay {DateTime.Today:dd/MM/yyyy}",
                Period.ThisMonth => $"Tháng {DateTime.Today:MM/yyyy}",
                _ => "Toàn thời gian"
            };
            if (btnMonthThis != null) btnMonthThis.Text = $"{_periodLabel} ▾";

            try
            {
                _summary = _reportService.GetSummary(from, to);
                _byMovie = _reportService.GetRevenueByMovie(from, to);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn báo cáo: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetKpi(0, $"{_summary.TotalRevenue:N0} đ", _periodLabel);
            SetKpi(1, $"{_summary.TicketRevenue:N0} đ", $"{_summary.TicketsSold:N0} vé đã bán");
            SetKpi(2, $"{_summary.ComboRevenue:N0} đ", "Doanh thu bắp nước");
            SetKpi(3, $"{_summary.SuccessfulTransactions:N0} ĐƠN", $"Kiosk {_summary.KioskTransactions} • POS {_summary.PosTransactions}");

            dgvReports.Rows.Clear();
            decimal total = _summary.TotalRevenue;
            int stt = 1;
            foreach (var r in _byMovie)
            {
                double share = total > 0 ? (double)(r.TotalRevenue / total) * 100 : 0;
                dgvReports.Rows.Add(
                    stt.ToString("D2"),
                    r.MovieName,
                    $"{r.TicketRevenue:N0} đ",
                    r.ShowtimeCount.ToString(),
                    $"{r.TicketsSold:N0} vé",
                    r.TotalSeats > 0 ? $"{r.AverageOccupancy:F1}%" : "-",
                    $"{r.TotalRevenue:N0} đ",
                    $"{share:F1}%");
                stt++;
            }

            if (lblFooterStatus != null)
                lblFooterStatus.Text = $"🟢 {_periodLabel} • Chỉ tính hóa đơn thanh toán thành công • Cập nhật {DateTime.Now:HH:mm:ss}";

            pnlBarChart.Invalidate();
            pnlDonutChart.Invalidate();
        }

        private void SetKpi(int index, string value, string sub)
        {
            if (kpiCards[index]?.Controls[$"lblKpiVal{index}"] is Label val) val.Text = value;
            if (kpiCards[index]?.Controls[$"lblKpiSub{index}"] is Label s) s.Text = sub;
        }

        private void PnlBarChart_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var items = _byMovie.Where(m => m.TotalRevenue > 0).Take(8).ToList();
            var area = new Rectangle(20, 40, pnlBarChart.Width - 40, pnlBarChart.Height - 60);

            using var muted = new SolidBrush(ColorTextMuted);
            using var smallFont = new Font("Segoe UI", 7.5F);
            if (items.Count == 0)
            {
                g.DrawString("Chưa có doanh thu trong kỳ này.", smallFont, muted, area.X, area.Y + 20);
                return;
            }

            decimal max = items.Max(i => i.TotalRevenue);
            int slot = area.Width / items.Count;
            int barWidth = Math.Min(60, slot - 16);
            using var barBrush = new SolidBrush(ColorPrimaryCyan);
            using var valueBrush = new SolidBrush(ColorAccentAmber);

            for (int i = 0; i < items.Count; i++)
            {
                int h = (int)((double)(items[i].TotalRevenue / max) * (area.Height - 40));
                int x = area.X + i * slot + (slot - barWidth) / 2;
                int y = area.Bottom - 20 - h;
                g.FillRectangle(barBrush, x, y, barWidth, h);

                string value = items[i].TotalRevenue >= 1_000_000 ? $"{items[i].TotalRevenue / 1_000_000m:0.#}tr" : $"{items[i].TotalRevenue / 1000m:0}k";
                g.DrawString(value, smallFont, valueBrush, x, y - 14);

                string name = items[i].MovieName.Length > 14 ? items[i].MovieName[..13] + "…" : items[i].MovieName;
                g.DrawString(name, smallFont, muted, new RectangleF(area.X + i * slot, area.Bottom - 18, slot, 16),
                    new StringFormat { Alignment = StringAlignment.Center });
            }
        }

        private void PnlDonutChart_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int kiosk = _summary.KioskTransactions;
            int pos = _summary.PosTransactions;
            int total = kiosk + pos;

            using var muted = new SolidBrush(ColorTextMuted);
            using var font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            var rect = new Rectangle(30, 50, 130, 130);

            if (total == 0)
            {
                using var empty = new Pen(Color.FromArgb(40, 255, 255, 255), 22);
                g.DrawEllipse(empty, rect);
                g.DrawString("Chưa có giao dịch", font, muted, 190, 105);
                return;
            }

            float kioskAngle = 360f * kiosk / total;
            using (var penKiosk = new Pen(ColorAccentAmber, 22))
            using (var penPos = new Pen(ColorPrimaryCyan, 22))
            {
                if (kiosk > 0) g.DrawArc(penKiosk, rect, -90, kioskAngle);
                if (pos > 0) g.DrawArc(penPos, rect, -90 + kioskAngle, 360 - kioskAngle);
            }

            using var amber = new SolidBrush(ColorAccentAmber);
            using var cyan = new SolidBrush(ColorPrimaryCyan);
            g.FillRectangle(amber, 190, 80, 12, 12);
            g.DrawString($"Kiosk: {kiosk} đơn ({100.0 * kiosk / total:F0}%)", font, Brushes.White, 208, 77);
            g.FillRectangle(cyan, 190, 115, 12, 12);
            g.DrawString($"POS: {pos} đơn ({100.0 * pos / total:F0}%)", font, Brushes.White, 208, 112);
        }
    }
}
