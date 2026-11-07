using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmAdminCombos : Form
    {
        private readonly Color COLOR_CANVAS = ColorTranslator.FromHtml("#101418");
        private readonly Color COLOR_PANEL_BG = ColorTranslator.FromHtml("#181C20");
        private readonly Color COLOR_CARD_BG = ColorTranslator.FromHtml("#22262B");
        private readonly Color COLOR_BORDER = ColorTranslator.FromHtml("#2C3238");
        private readonly Color COLOR_PRIMARY = ColorTranslator.FromHtml("#0284C7");
        private readonly Color COLOR_GOLD = ColorTranslator.FromHtml("#F59E0B");
        private readonly Color COLOR_TEXT_PRIMARY = ColorTranslator.FromHtml("#FFFFFF");
        private readonly Color COLOR_TEXT_SECONDARY = ColorTranslator.FromHtml("#94A3B8");
        private readonly Color COLOR_TEXT_MUTED = ColorTranslator.FromHtml("#64748B");

        private Panel pnlSidebar;
        private Panel pnlTopHeader;
        private Panel pnlRightInspector;
        private Panel pnlFooter;
        private Panel pnlCenter;
        private DataGridView dgvConcessions;

        public FrmAdminCombos()
        {
            InitializeComponent();
            SetupMasterLayout();
            BuildSidebar();
            BuildTopHeader();
            BuildRightInspector();
            BuildFooterBar();
            BuildCenterContent();
        }

        private void SetupMasterLayout()
        {
            this.Text = "CINELUXE MANAGEMENT - Quản lý kho combo bắp nước [FrmAdminCombos]";
            this.Size = new Size(1440, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = COLOR_CANVAS;

            pnlSidebar = new Panel
            {
                Width = 240,
                Dock = DockStyle.Left,
                BackColor = COLOR_PANEL_BG,
                Padding = new Padding(12)
            };

            pnlRightInspector = new Panel
            {
                Width = 390,
                Dock = DockStyle.Right,
                BackColor = COLOR_PANEL_BG,
                Padding = new Padding(16)
            };

            pnlFooter = new Panel
            {
                Height = 40,
                Dock = DockStyle.Bottom,
                BackColor = COLOR_PANEL_BG,
                Padding = new Padding(15, 8, 15, 8)
            };

            pnlTopHeader = new Panel
            {
                Height = 70,
                Dock = DockStyle.Top,
                BackColor = COLOR_PANEL_BG,
                Padding = new Padding(15)
            };

            pnlCenter = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = COLOR_CANVAS,
                Padding = new Padding(15)
            };

            this.Controls.Add(pnlCenter);
            this.Controls.Add(pnlTopHeader);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlRightInspector);
            this.Controls.Add(pnlSidebar);
        }

        private void BuildSidebar()
        {
            Label lblBrand = new Label
            {
                Text = "CINELUXE MANAGEMENT",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                Dock = DockStyle.Top,
                Height = 40
            };

            Label lblSection = new Label
            {
                Text = "PHẦN HỆ VẬN HÀNH",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = COLOR_TEXT_MUTED,
                Dock = DockStyle.Top,
                Height = 25
            };

            Button btnNavPOS = CreateNavButton("Bán vé Quầy POS\nFrmStaffPOS", false);
            Button btnNavCombo = CreateNavButton("Quản lý Combo Bắp Nước\nFrmAdminCombos", true);
            Button btnNavReport = CreateNavButton("Tài chính & Doanh thu\nFrmAdminReports", false);

            pnlSidebar.Controls.Add(btnNavReport);
            pnlSidebar.Controls.Add(btnNavCombo);
            pnlSidebar.Controls.Add(btnNavPOS);
            pnlSidebar.Controls.Add(lblSection);
            pnlSidebar.Controls.Add(lblBrand);
        }

        private Button CreateNavButton(string text, bool isActive)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9f, isActive ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = isActive ? COLOR_TEXT_PRIMARY : COLOR_TEXT_SECONDARY,
                BackColor = isActive ? COLOR_PRIMARY : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Top,
                Height = 48,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 4)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void BuildTopHeader()
        {
            Button btnAddCombo = new Button
            {
                Text = "+ Thêm Combo Mới (Ctrl+N)",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = COLOR_GOLD,
                Size = new Size(200, 36),
                Location = new Point(15, 16),
                FlatStyle = FlatStyle.Flat
            };
            btnAddCombo.FlatAppearance.BorderSize = 0;

            pnlTopHeader.Controls.Add(btnAddCombo);
        }

        private void BuildCenterContent()
        {
            Panel pnlTableContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = COLOR_CARD_BG,
                Padding = new Padding(10)
            };

            dgvConcessions = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = COLOR_CARD_BG,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvConcessions.Columns.Add("SKU", "MÃ SKU");
            dgvConcessions.Columns.Add("Name", "TÊN MÓN / COMBO");
            dgvConcessions.Columns.Add("Category", "PHÂN LOẠI");
            dgvConcessions.Columns.Add("CostPrice", "GIÁ VỐN");
            dgvConcessions.Columns.Add("SalePrice", "GIÁ NIÊM YẾT");

            dgvConcessions.Rows.Add("SKU-CB-001", "Combo Couple IMAX Gold\n1 Bắp ngọt 85oz + 2 Pepsi 32oz", "Combo tiết kiệm", "38.000 đ", "145.000 đ");
            dgvConcessions.Rows.Add("SKU-CB-002", "Combo Solo Gold\n1 Bắp mặn 64oz + 1 Coca 22oz", "Combo tiết kiệm", "24.500 đ", "95.000 đ");
            dgvConcessions.Rows.Add("SKU-PC-003", "Bắp Phô Mai Savory 46oz\nBột phô mai nhập khẩu New Zealand", "Bắp rang bơ", "15.200 đ", "65.000 đ");
            dgvConcessions.Rows.Add("SKU-PC-004", "Bắp Caramel Royal 64oz\nCaramel thắng nâu hạt tròn Mỹ", "Bắp rang bơ", "19.000 đ", "79.000 đ");
            dgvConcessions.Rows.Add("SKU-DK-005", "Coca-Cola Tươi 32oz Lớn\nPost-mix CO2 tiêu chuẩn 4.2 Bar", "Nước giải khát", "6.500 đ", "39.000 đ");

            dgvConcessions.DefaultCellStyle.BackColor = COLOR_CARD_BG;
            dgvConcessions.DefaultCellStyle.ForeColor = COLOR_TEXT_PRIMARY;
            dgvConcessions.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvConcessions.ColumnHeadersDefaultCellStyle.BackColor = COLOR_PANEL_BG;
            dgvConcessions.ColumnHeadersDefaultCellStyle.ForeColor = COLOR_TEXT_MUTED;
            dgvConcessions.EnableHeadersVisualStyles = false;

            pnlTableContainer.Controls.Add(dgvConcessions);
            pnlCenter.Controls.Add(pnlTableContainer);
        }

        private void BuildRightInspector()
        {
            Label lblInspectorTitle = new Label
            {
                Text = "Chi tiết Mặt hàng F&B    MODE: EDIT",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                Dock = DockStyle.Top,
                Height = 30
            };

            Label lblSkuName = new Label
                { Text = "Mã SKU\nSKU-CB-001", Font = new Font("JetBrains Mono", 8.5f), ForeColor = COLOR_TEXT_SECONDARY, Location = new Point(16, 80), AutoSize = true };

            Label lblProdName = new Label
                { Text = "Tên sản phẩm / Combo *\nCombo Couple IMAX Gold", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = COLOR_TEXT_PRIMARY, Location = new Point(180, 80), AutoSize = true };

            Button btnSave = new Button
            {
                Text = "Lưu thay đổi (Ctrl+S)",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                BackColor = COLOR_PRIMARY,
                Dock = DockStyle.Bottom,
                Height = 42,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;

            pnlRightInspector.Controls.Add(btnSave);
            pnlRightInspector.Controls.Add(lblProdName);
            pnlRightInspector.Controls.Add(lblSkuName);
            pnlRightInspector.Controls.Add(lblInspectorTitle);
        }

        private void BuildFooterBar()
        {
            Label lblFooterStatus = new Label
            {
                Text = "Tổng mặt hàng: 18 sản phẩm F&B  |  Giá trị tồn kho ước tính: 42.500.000 đ  |  Cảnh báo tồn min: 2 mặt hàng  |  SQLite Concurrency: OK (WAL Mode)",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = COLOR_TEXT_SECONDARY,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlFooter.Controls.Add(lblFooterStatus);
        }
    }
}