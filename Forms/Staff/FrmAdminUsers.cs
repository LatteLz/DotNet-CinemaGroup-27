using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmAdminUsers : Form
    {
        private Panel pnlTopBar;
        private Panel pnlSidebar;
        private Panel pnlMainContent;
        private DataGridView dgvUsers;

        public FrmAdminUsers()
        {
            InitializeComponent();
            SetupWindow();
            BuildUI();
            AddQuickNavigationButtons();
        }

        private void SetupWindow()
        {
            this.Text = "CINELUXE MANAGEMENT STUDIO - [FrmAdminUsers]";
            this.Size = new Size(1440, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ColorTranslator.FromHtml("#101418");
            this.DoubleBuffered = true;
        }

        private void BuildUI()
        {
            pnlTopBar = new Panel();
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Height = 36;
            pnlTopBar.BackColor = ColorTranslator.FromHtml("#0b0f12");

            Label lblAppTitle = new Label();
            lblAppTitle.Text = "  CINELUXE MANAGEMENT";
            lblAppTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Dock = DockStyle.Left;
            lblAppTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblAppTitle.Width = 240;

            Label lblCluster = new Label();
            lblCluster.Text = "● CLUSTER-APAC-01";
            lblCluster.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblCluster.ForeColor = ColorTranslator.FromHtml("#38bdf8");
            lblCluster.Location = new Point(1150, 8);
            lblCluster.AutoSize = true;

            Button btnClose = new Button();
            btnClose.Text = "✕";
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.ForeColor = Color.White;
            btnClose.BackColor = Color.Transparent;
            btnClose.Size = new Size(45, 36);
            btnClose.Dock = DockStyle.Right;
            btnClose.Click += (s, e) => Application.Exit();

            Button btnMaximize = new Button();
            btnMaximize.Text = "🗖";
            btnMaximize.FlatStyle = FlatStyle.Flat;
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.ForeColor = Color.White;
            btnMaximize.BackColor = Color.Transparent;
            btnMaximize.Size = new Size(45, 36);
            btnMaximize.Dock = DockStyle.Right;

            Button btnMinimize = new Button();
            btnMinimize.Text = "🗕";
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.ForeColor = Color.White;
            btnMinimize.BackColor = Color.Transparent;
            btnMinimize.Size = new Size(45, 36);
            btnMinimize.Dock = DockStyle.Right;

            pnlTopBar.Controls.Add(lblCluster);
            pnlTopBar.Controls.Add(btnClose);
            pnlTopBar.Controls.Add(btnMaximize);
            pnlTopBar.Controls.Add(btnMinimize);
            pnlTopBar.Controls.Add(lblAppTitle);

            pnlSidebar = new Panel();
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 240;
            pnlSidebar.BackColor = ColorTranslator.FromHtml("#14181d");

            string[] menuItems = { "Tổng quan", "Quản lý phim", "Phòng chiếu", "Lịch chiếu", "Combo & Bắp nước", "Quản lý người dùng", "Báo cáo doanh thu" };
            int menuTop = 20;
            foreach (var item in menuItems)
            {
                Button btnMenu = new Button();
                btnMenu.Text = "   " + item;
                btnMenu.TextAlign = ContentAlignment.MiddleLeft;
                btnMenu.FlatStyle = FlatStyle.Flat;
                btnMenu.FlatAppearance.BorderSize = 0;
                btnMenu.Size = new Size(240, 40);
                btnMenu.Location = new Point(0, menuTop);
                btnMenu.Font = new Font("Segoe UI", 9F, item == "Quản lý người dùng" ? FontStyle.Bold : FontStyle.Regular);
                btnMenu.BackColor = item == "Quản lý người dùng" ? ColorTranslator.FromHtml("#0284c7") : Color.Transparent;
                btnMenu.ForeColor = Color.White;

                string menuName = item;
                btnMenu.Click += (s, e) => {
                    if (menuName == "Combo & Bắp nước")
                    {
                        FrmAdminCombos comboForm = new FrmAdminCombos();
                        comboForm.Show();
                        this.Hide();
                    }
                    else if (menuName == "Tổng quan")
                    {
                        FrmStaffPOS posForm = new FrmStaffPOS();
                        posForm.Show();
                        this.Hide();
                    }
                    else if (menuName == "Quản lý người dùng")
                    {
                        MessageBox.Show("Bạn đang ở màn hình Quản lý người dùng.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Đang chuyển đến phân hệ: {menuName}", "Điều Hướng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                };

                pnlSidebar.Controls.Add(btnMenu);
                menuTop += 45;
            }

            Panel pnlStatus = new Panel();
            pnlStatus.Dock = DockStyle.Bottom;
            pnlStatus.Height = 32;
            pnlStatus.BackColor = ColorTranslator.FromHtml("#0b0f12");

            Label lblStatusText = new Label();
            lblStatusText.Text = "  SYSTEM MEM: 312 MB    |    ● WAL SYNCHRONIZED | Đồng bộ SQLite lần cuối: Hôm nay lúc 14:32:18    |    cineluxe_master.db (48.6 MB)    |    PAGE BUFFER: 4,096 KB";
            lblStatusText.Font = new Font("Segoe UI", 8F);
            lblStatusText.ForeColor = ColorTranslator.FromHtml("#94a3b8");
            lblStatusText.Dock = DockStyle.Fill;
            lblStatusText.TextAlign = ContentAlignment.MiddleLeft;
            pnlStatus.Controls.Add(lblStatusText);

            pnlMainContent = new Panel();
            pnlMainContent.Dock = DockStyle.Fill;
            pnlMainContent.BackColor = ColorTranslator.FromHtml("#101418");
            pnlMainContent.Padding = new Padding(20);

            Label lblBreadcrumb = new Label();
            lblBreadcrumb.Text = "HỆ THỐNG ĐIỀU HÀNH / PHÂN QUYỀN & TÀI KHOẢN / TABLE_USER_ACCOUNTS";
            lblBreadcrumb.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblBreadcrumb.ForeColor = ColorTranslator.FromHtml("#38bdf8");
            lblBreadcrumb.Location = new Point(20, 15);
            lblBreadcrumb.AutoSize = true;

            Label lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ NGƯỜI DÙNG";
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(18, 35);
            lblTitle.AutoSize = true;

            Label lblSysAuth = new Label();
            lblSysAuth.Text = "SYS_AUTH_V8";
            lblSysAuth.Font = new Font("Consolas", 8F, FontStyle.Bold);
            lblSysAuth.ForeColor = ColorTranslator.FromHtml("#f59e0b");
            lblSysAuth.BackColor = ColorTranslator.FromHtml("#272011");
            lblSysAuth.Location = new Point(275, 48);
            lblSysAuth.Padding = new Padding(4);
            lblSysAuth.AutoSize = true;

            Label lblRbac = new Label();
            lblRbac.Text = "🛡️ BẢO MẬT PHÂN QUYỀN RBAC-LEVEL 4 [ACTIVE]";
            lblRbac.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRbac.ForeColor = ColorTranslator.FromHtml("#4ade80");
            lblRbac.Location = new Point(560, 48);
            lblRbac.AutoSize = true;

            CreateKpiCards();
            CreateToolbar();
            CreateDataGridView();

            pnlMainContent.Controls.Add(lblBreadcrumb);
            pnlMainContent.Controls.Add(lblTitle);
            pnlMainContent.Controls.Add(lblSysAuth);
            pnlMainContent.Controls.Add(lblRbac);

            this.Controls.Add(pnlMainContent);
            this.Controls.Add(pnlSidebar);
            this.Controls.Add(pnlTopBar);
            this.Controls.Add(pnlStatus);
        }

        private void AddQuickNavigationButtons()
        {
            Button btnGoPos = new Button();
            btnGoPos.Text = "🖥️ Màn Hình POS";
            btnGoPos.Size = new Size(130, 30);
            btnGoPos.Location = new Point(915, 12);
            btnGoPos.BackColor = ColorTranslator.FromHtml("#1E293B");
            btnGoPos.ForeColor = Color.White;
            btnGoPos.FlatStyle = FlatStyle.Flat;
            btnGoPos.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#38BDF8");
            btnGoPos.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnGoPos.Click += (s, e) => {
                FrmStaffPOS posForm = new FrmStaffPOS();
                posForm.Show();
                this.Hide();
            };

            Button btnGoCombos = new Button();
            btnGoCombos.Text = "🍿 Quản Lý Combo";
            btnGoCombos.Size = new Size(140, 30);
            btnGoCombos.Location = new Point(1055, 12);
            btnGoCombos.BackColor = ColorTranslator.FromHtml("#0284C7");
            btnGoCombos.ForeColor = Color.White;
            btnGoCombos.FlatStyle = FlatStyle.Flat;
            btnGoCombos.FlatAppearance.BorderSize = 0;
            btnGoCombos.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnGoCombos.Click += (s, e) => {
                FrmAdminCombos comboForm = new FrmAdminCombos();
                comboForm.Show();
                this.Hide();
            };

            pnlTopBar.Controls.Add(btnGoPos);
            pnlTopBar.Controls.Add(btnGoCombos);
            btnGoPos.BringToFront();
            btnGoCombos.BringToFront();
        }

        private void CreateKpiCards()
        {
            string[] titles = { "TỔNG TÀI KHOẢN", "QUẢN TRỊ VIÊN (ADMIN)", "NHÂN VIÊN TRỰC CA (STAFF)", "HỘI VIÊN (CUSTOMERS)" };
            string[] values = { "1,428", "4", "24", "1,400" };
            string[] subs = { "↗ +12 tuần này", "Toàn quyền hệ thống", "18 Đang trực ca", "Tích lũy điểm" };
            int startX = 20;

            for (int i = 0; i < 4; i++)
            {
                Panel card = new Panel();
                card.Size = new Size(280, 75);
                card.Location = new Point(startX, 85);
                card.BackColor = ColorTranslator.FromHtml("#161b22");

                Label lblT = new Label();
                lblT.Text = titles[i];
                lblT.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                lblT.ForeColor = ColorTranslator.FromHtml("#94a3b8");
                lblT.Location = new Point(12, 10);
                lblT.AutoSize = true;

                Label lblV = new Label();
                lblV.Text = values[i];
                lblV.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
                lblV.ForeColor = Color.White;
                lblV.Location = new Point(10, 28);
                lblV.AutoSize = true;

                Label lblS = new Label();
                lblS.Text = subs[i];
                lblS.Font = new Font("Segoe UI", 8F);
                lblS.ForeColor = ColorTranslator.FromHtml("#38bdf8");
                lblS.Location = new Point(150, 45);
                lblS.AutoSize = true;

                card.Controls.Add(lblT);
                card.Controls.Add(lblV);
                card.Controls.Add(lblS);
                pnlMainContent.Controls.Add(card);

                startX += 292;
            }
        }

        private void CreateToolbar()
        {
            TextBox txtSearch = new TextBox();
            txtSearch.Text = "🔍 Tìm kiếm theo tên, email, username hoặc SĐT...";
            txtSearch.Size = new Size(320, 32);
            txtSearch.Location = new Point(20, 175);
            txtSearch.BackColor = ColorTranslator.FromHtml("#0b0f12");
            txtSearch.ForeColor = ColorTranslator.FromHtml("#94a3b8");
            txtSearch.Font = new Font("Segoe UI", 9F);

            ComboBox cbRole = new ComboBox();
            cbRole.Text = "Tất cả vai trò";
            cbRole.Size = new Size(160, 30);
            cbRole.Location = new Point(355, 175);
            cbRole.BackColor = ColorTranslator.FromHtml("#0b0f12");
            cbRole.ForeColor = Color.White;

            ComboBox cbStatus = new ComboBox();
            cbStatus.Text = "Tất cả trạng thái";
            cbStatus.Size = new Size(150, 30);
            cbStatus.Location = new Point(525, 175);
            cbStatus.BackColor = ColorTranslator.FromHtml("#0b0f12");
            cbStatus.ForeColor = Color.White;

            Button btnRefresh = new Button();
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.Size = new Size(95, 32);
            btnRefresh.Location = new Point(690, 174);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#1e293b");
            btnRefresh.ForeColor = ColorTranslator.FromHtml("#cbd5e1");
            btnRefresh.BackColor = ColorTranslator.FromHtml("#181c20");

            Button btnExport = new Button();
            btnExport.Text = "📊 Xuất Excel/CSV";
            btnExport.Size = new Size(125, 32);
            btnExport.Location = new Point(795, 174);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#1e293b");
            btnExport.ForeColor = ColorTranslator.FromHtml("#cbd5e1");
            btnExport.BackColor = ColorTranslator.FromHtml("#181c20");

            Button btnAdd = new Button();
            btnAdd.Text = "+ Thêm Người Dùng Mới";
            btnAdd.Size = new Size(185, 32);
            btnAdd.Location = new Point(995, 174);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAdd.ForeColor = Color.Black;
            btnAdd.BackColor = ColorTranslator.FromHtml("#f59e0b");

            pnlMainContent.Controls.Add(txtSearch);
            pnlMainContent.Controls.Add(cbRole);
            pnlMainContent.Controls.Add(cbStatus);
            pnlMainContent.Controls.Add(btnRefresh);
            pnlMainContent.Controls.Add(btnExport);
            pnlMainContent.Controls.Add(btnAdd);
        }

        private void CreateDataGridView()
        {
            dgvUsers = new DataGridView();
            dgvUsers.Location = new Point(20, 220);
            dgvUsers.Size = new Size(1160, 560);
            dgvUsers.BackgroundColor = ColorTranslator.FromHtml("#14181d");
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#101418");
            dgvUsers.DefaultCellStyle.ForeColor = Color.White;
            dgvUsers.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1e293b");
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#181c20");
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#94a3b8");
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.RowTemplate.Height = 50;

            dgvUsers.Columns.Add("colId", "MÃ ID");
            dgvUsers.Columns.Add("colUser", "NGƯỜI DÙNG / TÀI KHOẢN");
            dgvUsers.Columns.Add("colEmail", "LIÊN HỆ EMAIL");
            dgvUsers.Columns.Add("colPhone", "SỐ ĐIỆN THOẠI");
            dgvUsers.Columns.Add("colRole", "VAI TRÒ / NHIỆM VỤ");
            dgvUsers.Columns.Add("colStatus", "TRẠNG THÁI");
            dgvUsers.Columns.Add("colDate", "NGÀY TẠO / ĐĂNG NHẬP CUỐI");
            dgvUsers.Columns.Add("colAction", "THAO TÁC");

            dgvUsers.Columns[0].Width = 90;
            dgvUsers.Columns[1].Width = 240;
            dgvUsers.Columns[2].Width = 200;
            dgvUsers.Columns[3].Width = 110;
            dgvUsers.Columns[4].Width = 170;
            dgvUsers.Columns[5].Width = 100;
            dgvUsers.Columns[6].Width = 175;
            dgvUsers.Columns[7].Width = 75;

            dgvUsers.Rows.Add("#USR-801", "Nguyễn Hoàng Nam\n@hoangnam.admin", "hoangnam.adn@cineluxe.vn", "0983 841 992", "QUẢN TRỊ CẤP CAO", "● Hoạt động", "12/01/2023 - 08:30\nHôm nay 14:15", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-804", "Trần Thị Thu Hà\n@thuha.cinemangr", "thuha.cinemangr@gmail.com", "0912 455 889", "TRƯỞNG CỤM RẠP", "● Hoạt động", "05/03/2023 - 09:12\nHôm qua 19:48", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-818", "Lê Minh Tuấn\n@tuan.pos01", "minhtuan.pos@cineluxe.vn", "0938 112 344", "THU NGÂN QUẦY VÉ", "● Hoạt động", "15/07/2023 - 14:00\nHôm nay 13:00 (Trực ca)", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-822", "Phạm Phương Anh\n@phuonganh.fnb", "phuonganh.fnb@cineluxe.vn", "0971 668 201", "KHO BẮP NƯỚC F&B", "● Hoạt động", "02/09/2023 - 10:20\nHôm nay 11:45 (Trực ca)", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-831", "Vũ Đình Trọng\n@trong.projection", "trong.kythuat@cineluxe.vn", "0944 883 129", "KỸ THUẬT PHÒNG CHIẾU", "● Đã khóa", "10/11/2023 - 16:45\n28/09/2024 - 18:28", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-104", "Đặng Hoàng Yến\n@hoangyen.vip", "hoangyen.creative@agency.com", "0988 765 432", "VIP DIAMOND", "● Hoạt động", "04/01/2024 - 21:10\nHôm nay 12:20 (Đặt vé)", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-219", "Bùi Kim Ngân\n@kimngan.bui", "ngan.buikim@outlook.com", "0989 234 567", "HỘI VIÊN GOLD", "● Hoạt động", "18/02/2024 - 11:15\n12/10/2024 - 28:05", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-345", "Hoàng Quốc Bảo\n@bao.hq99", "quocbao.dev@fpt.edu.vn", "0932 774 918", "HỘI VIÊN SILVER", "● Hoạt động", "20/03/2024 - 15:40\n09/10/2024 - 17:30", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-402", "Phan Văn Đạt\n@dat.phan98", "dat.phan98@yahoo.com", "0908 654 321", "THÀNH VIÊN MỚI", "● Đã khóa (Spam)", "05/05/2024 - 09:00\n14/09/2024 - 03:15", "👁 ✏ 🔒");
            dgvUsers.Rows.Add("#USR-856", "Lương Triệu Vỹ\n@vy.shiftleader", "vy.luong@cineluxe.vn", "0977 128 459", "TRƯỞNG CA TRỰC (LEAD)", "● Hoạt động", "12/12/2023 - 07:45\nHôm nay 14:32 (Trực ca)", "👁 ✏ 🔒");

            pnlMainContent.Controls.Add(dgvUsers);
        }
    }
}