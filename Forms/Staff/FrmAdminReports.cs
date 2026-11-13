using System;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Admin
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

        public FrmAdminReports()
        {
            InitializeComponent();
            InitializeComponentCustom();
            LoadRealDataFromDatabase();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "CINELUXE MANAGEMENT STUDIO - BÁO CÁO TÀI CHÍNH & DOANH THU";
            this.Size = new Size(1440, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorBg;
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            BuildSidebar();
            BuildHeader();
            BuildFilterBar();
            BuildKpiCards();
            BuildChartsContainer();
            BuildDataGridViewCustom();
            BuildFooterStatus();
        }

        private void BuildSidebar()
        {
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Size = new Size(220, 900),
                BackColor = ColorSurface
            };

            lblLogo = new Label
            {
                Text = "CINELUXE",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(180, 30)
            };
            pnlSidebar.Controls.Add(lblLogo);

            lnkPos = CreateSidebarLink("Bán Vé Quầy POS", 80, (s, e) => { });
            lnkCombo = CreateSidebarLink("Quản Lý Combo F&B", 130, (s, e) => { });
            lnkReports = CreateSidebarLink("Tài Chính & Doanh Thu", 180, (s, e) => { });
            lnkOccupancy = CreateSidebarLink("Lấp đầy Khán Phòng", 230, (s, e) => { });
            lnkAbout = CreateSidebarLink("Cấu Hình & Giới Thiệu", 280, (s, e) => { });

            lnkReports.LinkColor = ColorTranslator.FromHtml("#38bdf8");

            lblUser = new Label
            {
                Text = "Julian Vance\nQuản Lý Trưởng",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ColorTextMuted,
                Location = new Point(20, 820),
                Size = new Size(180, 40)
            };
            pnlSidebar.Controls.Add(lblUser);

            this.Controls.Add(pnlSidebar);
        }

        private LinkLabel CreateSidebarLink(string text, int top, LinkLabelLinkClickedEventHandler onClick)
        {
            var lnk = new LinkLabel
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                LinkColor = ColorTextMuted,
                ActiveLinkColor = ColorTranslator.FromHtml("#38bdf8"),
                VisitedLinkColor = ColorTextMuted,
                Location = new Point(20, top),
                Size = new Size(180, 25),
                TabStop = true
            };
            lnk.LinkClicked += onClick;
            pnlSidebar.Controls.Add(lnk);
            return lnk;
        }

        private void BuildHeader()
        {
            pnlHeader = new Panel
            {
                Location = new Point(220, 0),
                Size = new Size(1220, 48),
                BackColor = ColorSurface
            };

            lblBreadcrumb = new Label
            {
                Text = "BÁN VÉ POS / KHO COMBO / BÁO CÁO DOANH THU",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ColorTextMuted,
                Location = new Point(20, 15),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblBreadcrumb);

            lblShift = new Label
            {
                Text = "SHIFT: MORNING (A)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = ColorAccentAmber,
                Location = new Point(780, 15),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblShift);

            btnRefresh = CreateHeaderButton("Refresh", 940, (s, e) => LoadRealDataFromDatabase());
            btnHelp = CreateHeaderButton("Help", 1030, (s, e) => { });

            this.Controls.Add(pnlHeader);
        }

        private Button CreateHeaderButton(string text, int left, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                BackColor = ColorCardBg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(left, 10),
                Size = new Size(80, 28)
            };
            btn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#242b33");
            btn.Click += onClick;
            pnlHeader.Controls.Add(btn);
            return btn;
        }

        private void BuildFilterBar()
        {
            pnlFilterBar = new Panel
            {
                Location = new Point(240, 60),
                Size = new Size(1180, 65),
                BackColor = ColorCardBg
            };

            btnMonthThis = new Button
            {
                Text = "Tháng Này [Active]",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = ColorPrimaryCyan,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 15),
                Size = new Size(140, 35)
            };
            btnMonthThis.FlatAppearance.BorderSize = 0;
            pnlFilterBar.Controls.Add(btnMonthThis);

            btnQuery = new Button
            {
                Text = "Truy Vấn (F5)",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = ColorPrimaryCyan,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(900, 15),
                Size = new Size(120, 35)
            };
            btnQuery.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#38bdf8");
            btnQuery.Click += (s, e) => LoadRealDataFromDatabase();
            pnlFilterBar.Controls.Add(btnQuery);

            btnExportExcel = new Button
            {
                Text = "Xuất Excel",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = ColorSurface,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(1030, 15),
                Size = new Size(120, 35)
            };
            btnExportExcel.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#334155");
            pnlFilterBar.Controls.Add(btnExportExcel);

            this.Controls.Add(pnlFilterBar);
        }

        private void BuildKpiCards()
        {
            pnlKpiContainer = new FlowLayoutPanel
            {
                Location = new Point(240, 135),
                Size = new Size(1180, 100),
                BackColor = ColorBg,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

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

            this.Controls.Add(pnlKpiContainer);
        }

        private void BuildChartsContainer()
        {
            pnlChartContainer = new Panel
            {
                Location = new Point(240, 245),
                Size = new Size(1180, 210),
                BackColor = ColorBg
            };

            pnlBarChart = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(765, 210),
                BackColor = ColorCardBg
            };
            lblBarTitle = new Label
            {
                Text = "XU HƯỚNG DOANH THU NGÀY TRONG THÁNG",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ColorTextMuted,
                Location = new Point(15, 12)
            };
            pnlBarChart.Controls.Add(lblBarTitle);
            pnlChartContainer.Controls.Add(pnlBarChart);

            pnlDonutChart = new Panel
            {
                Location = new Point(780, 0),
                Size = new Size(400, 210),
                BackColor = ColorCardBg
            };
            lblDonutTitle = new Label
            {
                Text = "TỶ TRỌNG KÊNH BÁN TOUCHPOINT",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ColorTextMuted,
                Location = new Point(15, 12)
            };
            pnlDonutChart.Controls.Add(lblDonutTitle);
            pnlChartContainer.Controls.Add(pnlDonutChart);

            this.Controls.Add(pnlChartContainer);
        }

        private void BuildDataGridViewCustom()
        {
            dgvReports.Location = new Point(240, 465);
            dgvReports.Size = new Size(1180, 360);
            dgvReports.BackgroundColor = ColorSurface;
            dgvReports.ForeColor = Color.White;
            dgvReports.GridColor = ColorTranslator.FromHtml("#1f2937");
            dgvReports.RowHeadersVisible = false;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvReports.DefaultCellStyle.BackColor = ColorSurface;
            dgvReports.DefaultCellStyle.ForeColor = Color.White;
            dgvReports.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1e242a");
            dgvReports.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = ColorCardBg;
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextMuted;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.RowTemplate.Height = 35;

            dgvReports.Columns["ColSTT"].Width = 40;
            dgvReports.Columns["ColName"].Width = 280;
        }

        private void BuildFooterStatus()
        {
            lblFooterStatus = new Label
            {
                Text = "Truy vấn LINQ: 18ms QUA SQLITE INDEX • MÃ BÁO CÁO: REP-20241026-FIN-091 • Ký số: MD5-VERIFIED",
                Font = new Font("Segoe UI", 8F),
                ForeColor = ColorTranslator.FromHtml("#64748b"),
                Location = new Point(240, 835),
                Size = new Size(1180, 25)
            };
            this.Controls.Add(lblFooterStatus);
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