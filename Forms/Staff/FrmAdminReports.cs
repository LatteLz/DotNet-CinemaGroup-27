using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using CinemaManagement.Forms.Staff;

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

        private string connectionString = "Data Source=cinema.db;Version=3;";
        private Panel[] kpiCards = new Panel[4];
        private ErrorProvider errorProvider = new ErrorProvider();

        public FrmAdminReports()
        {
            InitializeComponent();
            InitializeCustomEvents();
            LoadRealDataFromDatabase();
        }

        private void InitializeCustomEvents()
        {
            this.BackColor = ColorBg;
            this.ForeColor = Color.White;

            if (lnkPos != null)
            {
                lnkPos.LinkClicked += (s, e) => {
                    var posForm = new FrmStaffPOS();
                    posForm.Show();
                    this.Hide();
                };
            }

            if (lnkCombo != null)
            {
                lnkCombo.LinkClicked += (s, e) => {
                    var comboForm = new FrmAdminCombos();
                    comboForm.Show();
                    this.Hide();
                };
            }

            if (lnkReports != null)
            {
                lnkReports.LinkClicked += (s, e) => { };
            }

            if (lnkOccupancy != null)
            {
                lnkOccupancy.LinkClicked += (s, e) => {
                    var occupancyForm = new FrmOccupancyReport();
                    occupancyForm.Show();
                    this.Hide();
                };
            }

            if (lnkUsers != null)
            {
                lnkUsers.LinkClicked += (s, e) => {
                    var usersForm = new FrmAdminUsers();
                    usersForm.Show();
                    this.Hide();
                };
            }

            if (btnQuery != null)
            {
                btnQuery.Click += (s, e) => {
                    errorProvider.Clear();
                    LoadRealDataFromDatabase();
                    MessageBox.Show("Đã truy vấn dữ liệu tài chính & doanh thu mới nhất từ CSDL.", "Truy Vấn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
            }

            if (btnRefresh != null)
            {
                btnRefresh.Click += (s, e) => {
                    errorProvider.Clear();
                    LoadRealDataFromDatabase();
                    MessageBox.Show("Làm mới dữ liệu báo cáo thành công!", "Làm Mới", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                    sw.WriteLine("STT,TEN PHIM / COMBO,PHAN LOAI,SUAT CHIEU,TONG VE,LAP DAY TB,DOANH THU (VND),THOI GIAN GIAO DICH");
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
            try
            {
                decimal totalRevenue = 0;
                int totalTransactions = 0;
                decimal totalComboRevenue = 0;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string queryStats = "SELECT IFNULL(SUM(TotalAmount), 0) AS TotalRev, COUNT(Id) AS TotalTrans FROM Bookings;";
                    using (SQLiteCommand cmd = new SQLiteCommand(queryStats, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalRevenue = Convert.ToDecimal(reader["TotalRev"], CultureInfo.InvariantCulture);
                                totalTransactions = Convert.ToInt32(reader["TotalTrans"]);
                            }
                        }
                    }

                    string queryCombo = "SELECT IFNULL(SUM(Quantity * UnitPrice), 0) AS ComboRev FROM BookingCombos;";
                    using (SQLiteCommand cmd = new SQLiteCommand(queryCombo, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            totalComboRevenue = Convert.ToDecimal(result, CultureInfo.InvariantCulture);
                        }
                    }

                    decimal totalTicketRevenue = totalRevenue - totalComboRevenue;
                    if (totalTicketRevenue < 0) totalTicketRevenue = totalRevenue;

                    if (kpiCards[0]?.Controls["lblKpiVal0"] is Label lbl1) lbl1.Text = $"{totalRevenue:N0} đ";
                    if (kpiCards[0]?.Controls["lblKpiSub0"] is Label sub1) sub1.Text = "Đồng bộ từ bảng Bookings";

                    if (kpiCards[1]?.Controls["lblKpiVal1"] is Label lbl2) lbl2.Text = $"{totalTicketRevenue:N0} đ";
                    if (kpiCards[1]?.Controls["lblKpiSub1"] is Label sub2) sub2.Text = "Doanh thu thuần tiền vé";

                    if (kpiCards[2]?.Controls["lblKpiVal2"] is Label lbl3) lbl3.Text = $"{totalComboRevenue:N0} đ";
                    if (kpiCards[2]?.Controls["lblKpiSub2"] is Label sub3) sub3.Text = "Doanh thu từ BookingCombos";

                    if (kpiCards[3]?.Controls["lblKpiVal3"] is Label lbl4) lbl4.Text = $"{totalTransactions:N0} ĐƠN";
                    if (kpiCards[3]?.Controls["lblKpiSub3"] is Label sub4) sub4.Text = "Tổng số hóa đơn đặt vé";

                    string queryGrid = "SELECT MovieTitle, ShowtimeInfo, TotalAmount, CreatedAt FROM Bookings ORDER BY CreatedAt DESC;";
                    using (SQLiteCommand cmd = new SQLiteCommand(queryGrid, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            dgvReports.Rows.Clear();
                            int stt = 1;
                            while (reader.Read())
                            {
                                string movieTitle = reader["MovieTitle"]?.ToString() ?? "N/A";
                                string showtimeInfo = reader["ShowtimeInfo"]?.ToString() ?? "N/A";
                                
                                decimal amount = 0;
                                string rawAmount = reader["TotalAmount"]?.ToString() ?? "0";
                                decimal.TryParse(rawAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out amount);

                                string createdAt = reader["CreatedAt"]?.ToString() ?? DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                                dgvReports.Rows.Add(
                                    stt.ToString("D2"),
                                    movieTitle,
                                    "PHIM CHIẾU RẠP",
                                    showtimeInfo,
                                    "1 vé",
                                    "100%",
                                    $"{amount:N0} đ",
                                    createdAt
                                );
                                stt++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL lấy báo cáo: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}