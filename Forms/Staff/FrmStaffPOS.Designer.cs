namespace CinemaManagement.Forms.Staff
{
    partial class FrmStaffPOS
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTopNav = new System.Windows.Forms.Panel();
            this.lblSubNavMenu = new System.Windows.Forms.Label();
            this.lblShiftInfo = new System.Windows.Forms.Label();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblNavTitle = new System.Windows.Forms.Label();
            this.lnkPos = new System.Windows.Forms.LinkLabel();
            this.lnkCombo = new System.Windows.Forms.LinkLabel();
            this.lnkFinance = new System.Windows.Forms.LinkLabel();
            this.lnkOccupancy = new System.Windows.Forms.LinkLabel();
            this.lnkAbout = new System.Windows.Forms.LinkLabel();
            this.pnlUserProfile = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlLeftWork = new System.Windows.Forms.Panel();
            this.pnlMovieBar = new System.Windows.Forms.Panel();
            this.lblMovie1 = new System.Windows.Forms.Label();
            this.lblMovie2 = new System.Windows.Forms.Label();
            this.lblMovie3 = new System.Windows.Forms.Label();
            this.pnlShowtimeBar = new System.Windows.Forms.Panel();
            this.lblShowtime1 = new System.Windows.Forms.Label();
            this.lblShowtime2 = new System.Windows.Forms.Label();
            this.pnlTabsBar = new System.Windows.Forms.Panel();
            this.btnTabSeat = new System.Windows.Forms.Button();
            this.btnTabCombo = new System.Windows.Forms.Button();
            this.lblScreenHeader = new System.Windows.Forms.Label();
            this.pnlSeatMatrix = new System.Windows.Forms.Panel();
            this.pnlComboCatalog = new System.Windows.Forms.Panel();
            this.pnlSeatBottomStatus = new System.Windows.Forms.Panel();
            this.lblSelectedSeats = new System.Windows.Forms.Label();
            this.btnChangeSeat = new System.Windows.Forms.Button();
            this.pnlRightTerminal = new System.Windows.Forms.Panel();
            this.pnlCashierInfo = new System.Windows.Forms.Panel();
            this.lblCashierName = new System.Windows.Forms.Label();
            this.lblPosMachine = new System.Windows.Forms.Label();
            this.pnlCustomerInfo = new System.Windows.Forms.Panel();
            this.lblCustomerDetails = new System.Windows.Forms.Label();
            this.btnChangeCustomer = new System.Windows.Forms.Button();
            this.dgvOrderItems = new System.Windows.Forms.DataGridView();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTotalBlock = new System.Windows.Forms.Panel();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblVatInfo = new System.Windows.Forms.Label();
            this.pnlCashQuick = new System.Windows.Forms.Panel();
            this.lblCashTenderedTitle = new System.Windows.Forms.Label();
            this.btnCash200k = new System.Windows.Forms.Button();
            this.btnCash500k = new System.Windows.Forms.Button();
            this.btnCash1m = new System.Windows.Forms.Button();
            this.btnCashExact = new System.Windows.Forms.Button();
            this.txtCashGiven = new System.Windows.Forms.TextBox();
            this.lblChangeDue = new System.Windows.Forms.Label();
            this.pnlPaymentMethods = new System.Windows.Forms.Panel();
            this.btnPayCash = new System.Windows.Forms.Button();
            this.btnPayCard = new System.Windows.Forms.Button();
            this.btnPayQR = new System.Windows.Forms.Button();
            this.btnCheckout = new System.Windows.Forms.Button();
            this.pnlBottomActionButtons = new System.Windows.Forms.Panel();
            this.btnReprint = new System.Windows.Forms.Button();
            this.btnCancelOrder = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).BeginInit();
            this.pnlTopNav.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.pnlUserProfile.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlLeftWork.SuspendLayout();
            this.pnlMovieBar.SuspendLayout();
            this.pnlShowtimeBar.SuspendLayout();
            this.pnlTabsBar.SuspendLayout();
            this.pnlSeatBottomStatus.SuspendLayout();
            this.pnlRightTerminal.SuspendLayout();
            this.pnlCashierInfo.SuspendLayout();
            this.pnlCustomerInfo.SuspendLayout();
            this.pnlTotalBlock.SuspendLayout();
            this.pnlCashQuick.SuspendLayout();
            this.pnlPaymentMethods.SuspendLayout();
            this.pnlBottomActionButtons.SuspendLayout();
            this.SuspendLayout();

            this.ClientSize = new System.Drawing.Size(1440, 900);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlTopNav);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "FrmStaffPOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CINELUXE MANAGEMENT STUDIO - [.NET 8.0 / WPF-NATIVE ENGINE]";

            this.pnlTopNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopNav.Height = 45;
            this.pnlTopNav.BackColor = System.Drawing.Color.FromArgb(11, 15, 18);

            this.lblSubNavMenu.Text = "  BÁN VÉ POS    |    KHO COMBO    |    BÁO CÁO DOANH THU    |    HIỆU SUẤT KHÁN PHÒNG    |    HỆ THỐNG";
            this.lblSubNavMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubNavMenu.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblSubNavMenu.Location = new System.Drawing.Point(240, 12);
            this.lblSubNavMenu.AutoSize = true;

            this.lblShiftInfo.Text = "🕒 SHIFT: MORNING (A)";
            this.lblShiftInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblShiftInfo.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.lblShiftInfo.Location = new System.Drawing.Point(1230, 12);
            this.lblShiftInfo.AutoSize = true;

            this.pnlTopNav.Controls.Add(this.lblSubNavMenu);
            this.pnlTopNav.Controls.Add(this.lblShiftInfo);

            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 240;
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(20, 24, 29);

            this.lblLogo.Text = "CINELUXE\nEXCELLENCE CINEMA";
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(56, 189, 248);
            this.lblLogo.Location = new System.Drawing.Point(16, 16);
            this.lblLogo.Size = new System.Drawing.Size(200, 45);

            this.lblNavTitle.Text = "PHÂN HỆ VẬN HÀNH";
            this.lblNavTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNavTitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblNavTitle.Location = new System.Drawing.Point(16, 80);
            this.lblNavTitle.AutoSize = true;

            this.lnkPos.Text = "Bán vé Quầy POS\nFrmStaffPOS";
            this.lnkPos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lnkPos.LinkColor = System.Drawing.Color.FromArgb(56, 189, 248);
            this.lnkPos.Location = new System.Drawing.Point(16, 110);
            this.lnkPos.Size = new System.Drawing.Size(200, 40);
            this.lnkPos.TabStop = true;

            this.lnkCombo.Text = "Quản lý Combo Bắp Nước\nFrmAdminCombos";
            this.lnkCombo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkCombo.LinkColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lnkCombo.Location = new System.Drawing.Point(16, 165);
            this.lnkCombo.Size = new System.Drawing.Size(200, 40);
            this.lnkCombo.TabStop = true;

            this.lnkFinance.Text = "Tài chính & Doanh thu\nFrmAdminReports";
            this.lnkFinance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkFinance.LinkColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lnkFinance.Location = new System.Drawing.Point(16, 220);
            this.lnkFinance.Size = new System.Drawing.Size(200, 40);
            this.lnkFinance.TabStop = true;

            this.lnkOccupancy.Text = "Lấp đầy Khán phòng\nFrmOccupancyReport";
            this.lnkOccupancy.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkOccupancy.LinkColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lnkOccupancy.Location = new System.Drawing.Point(16, 275);
            this.lnkOccupancy.Size = new System.Drawing.Size(200, 40);
            this.lnkOccupancy.TabStop = true;

            this.lnkAbout.Text = "Cấu hình & Giới thiệu\nFrmAbout";
            this.lnkAbout.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkAbout.LinkColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lnkAbout.Location = new System.Drawing.Point(16, 360);
            this.lnkAbout.Size = new System.Drawing.Size(200, 40);
            this.lnkAbout.TabStop = true;

            this.pnlUserProfile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUserProfile.Height = 70;
            this.pnlUserProfile.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);

            this.lblUserName.Text = "Nhân viên POS";
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(12, 12);
            this.lblUserName.AutoSize = true;

            this.lblUserRole.Text = "Quản lý Trưởng";
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblUserRole.Location = new System.Drawing.Point(12, 34);
            this.lblUserRole.AutoSize = true;

            this.pnlUserProfile.Controls.Add(this.lblUserName);
            this.pnlUserProfile.Controls.Add(this.lblUserRole);

            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Controls.Add(this.lblNavTitle);
            this.pnlSidebar.Controls.Add(this.lnkPos);
            this.pnlSidebar.Controls.Add(this.lnkCombo);
            this.pnlSidebar.Controls.Add(this.lnkFinance);
            this.pnlSidebar.Controls.Add(this.lnkOccupancy);
            this.pnlSidebar.Controls.Add(this.lnkAbout);
            this.pnlSidebar.Controls.Add(this.pnlUserProfile);

            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);

            this.pnlLeftWork.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftWork.Width = 750;
            this.pnlLeftWork.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);
            this.pnlLeftWork.Padding = new System.Windows.Forms.Padding(12);

            this.pnlMovieBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMovieBar.Height = 65;
            this.pnlMovieBar.BackColor = System.Drawing.Color.FromArgb(20, 24, 29);

            this.lblMovie1.Text = "🔥 Dune: Part Two\n166 phút • Sci-Fi • IMAX Laser 4K";
            this.lblMovie1.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMovie1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.lblMovie1.Location = new System.Drawing.Point(10, 8);
            this.lblMovie1.Size = new System.Drawing.Size(235, 50);
            this.lblMovie1.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);

            this.lblMovie2.Text = "  Godzilla x Kong\n115 phút • Action • Phòng 03 VIP";
            this.lblMovie2.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblMovie2.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblMovie2.Location = new System.Drawing.Point(250, 8);
            this.lblMovie2.Size = new System.Drawing.Size(235, 50);
            this.lblMovie2.BackColor = System.Drawing.Color.FromArgb(24, 28, 35);

            this.lblMovie3.Text = "  18+ Mai (Trấn Thành)\n131 phút • Drama • Phòng 04 Standard";
            this.lblMovie3.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblMovie3.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblMovie3.Location = new System.Drawing.Point(490, 8);
            this.lblMovie3.Size = new System.Drawing.Size(235, 50);
            this.lblMovie3.BackColor = System.Drawing.Color.FromArgb(24, 28, 35);

            this.pnlMovieBar.Controls.Add(this.lblMovie1);
            this.pnlMovieBar.Controls.Add(this.lblMovie2);
            this.pnlMovieBar.Controls.Add(this.lblMovie3);

            this.pnlShowtimeBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlShowtimeBar.Height = 45;
            this.pnlShowtimeBar.BackColor = System.Drawing.Color.FromArgb(20, 24, 29);

            this.lblShowtime1.Text = "SUẤT CHIẾU: 19:30 • P.01 [IMAX Laser]";
            this.lblShowtime1.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblShowtime1.ForeColor = System.Drawing.Color.FromArgb(56, 189, 248);
            this.lblShowtime1.Location = new System.Drawing.Point(10, 12);
            this.lblShowtime1.AutoSize = true;

            this.lblShowtime2.Text = "20:15 • P.02 [VIP Suite]   |   21:48 • P.01 [IMAX]   |   Tỉ lệ lấp đầy: 78%";
            this.lblShowtime2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblShowtime2.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblShowtime2.Location = new System.Drawing.Point(320, 14);
            this.lblShowtime2.AutoSize = true;

            this.pnlShowtimeBar.Controls.Add(this.lblShowtime1);
            this.pnlShowtimeBar.Controls.Add(this.lblShowtime2);

            this.pnlTabsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabsBar.Height = 40;
            this.pnlTabsBar.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);

            this.btnTabSeat.Text = "💺 Sơ đồ Ghế (F6)";
            this.btnTabSeat.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnTabSeat.ForeColor = System.Drawing.Color.White;
            this.btnTabSeat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabSeat.Location = new System.Drawing.Point(10, 4);
            this.btnTabSeat.Size = new System.Drawing.Size(140, 32);

            this.btnTabCombo.Text = "🍿 Combo Bắp Nước (F7)";
            this.btnTabCombo.BackColor = System.Drawing.Color.FromArgb(24, 28, 35);
            this.btnTabCombo.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.btnTabCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabCombo.Location = new System.Drawing.Point(155, 4);
            this.btnTabCombo.Size = new System.Drawing.Size(165, 32);

            this.pnlTabsBar.Controls.Add(this.btnTabSeat);
            this.pnlTabsBar.Controls.Add(this.btnTabCombo);

            this.lblScreenHeader.Text = "MÀN HÌNH CHIẾU CONG CURVED LASER IMAX (CINELUXE SCREEN #01)";
            this.lblScreenHeader.Font = new System.Drawing.Font("Consolas", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblScreenHeader.ForeColor = System.Drawing.Color.FromArgb(56, 189, 248);
            this.lblScreenHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblScreenHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblScreenHeader.Height = 35;

            this.pnlSeatMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSeatMatrix.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);

            this.pnlComboCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlComboCatalog.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);
            this.pnlComboCatalog.AutoScroll = true;

            this.pnlSeatBottomStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSeatBottomStatus.Height = 45;
            this.pnlSeatBottomStatus.BackColor = System.Drawing.Color.FromArgb(20, 24, 29);

            this.lblSelectedSeats.Text = "🟢 Đang chọn: G07, G08 [Hàng G • Ghế VIP Center]";
            this.lblSelectedSeats.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSelectedSeats.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.lblSelectedSeats.Location = new System.Drawing.Point(12, 12);
            this.lblSelectedSeats.AutoSize = true;

            this.btnChangeSeat.Text = "🔄 Đổi Ghế (F4)";
            this.btnChangeSeat.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnChangeSeat.ForeColor = System.Drawing.Color.White;
            this.btnChangeSeat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeSeat.Location = new System.Drawing.Point(610, 6);
            this.btnChangeSeat.Size = new System.Drawing.Size(120, 32);

            this.pnlSeatBottomStatus.Controls.Add(this.lblSelectedSeats);
            this.pnlSeatBottomStatus.Controls.Add(this.btnChangeSeat);

            this.pnlLeftWork.Controls.Add(this.pnlComboCatalog);
            this.pnlLeftWork.Controls.Add(this.pnlSeatMatrix);
            this.pnlLeftWork.Controls.Add(this.lblScreenHeader);
            this.pnlLeftWork.Controls.Add(this.pnlSeatBottomStatus);
            this.pnlLeftWork.Controls.Add(this.pnlTabsBar);
            this.pnlLeftWork.Controls.Add(this.pnlShowtimeBar);
            this.pnlLeftWork.Controls.Add(this.pnlMovieBar);

            this.pnlRightTerminal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightTerminal.BackColor = System.Drawing.Color.FromArgb(20, 24, 29);
            this.pnlRightTerminal.Padding = new System.Windows.Forms.Padding(12);

            this.pnlCashierInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCashierInfo.Height = 35;
            this.lblCashierName.Text = "Thu ngân: Nhân viên POS (Ca tối #2)";
            this.lblCashierName.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCashierName.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCashierName.Location = new System.Drawing.Point(0, 5);
            this.lblCashierName.AutoSize = true;

            this.lblPosMachine.Text = "Máy POS: #01-A";
            this.lblPosMachine.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPosMachine.ForeColor = System.Drawing.Color.FromArgb(56, 189, 248);
            this.lblPosMachine.Location = new System.Drawing.Point(340, 5);
            this.lblPosMachine.AutoSize = true;

            this.pnlCashierInfo.Controls.Add(this.lblCashierName);
            this.pnlCashierInfo.Controls.Add(this.lblPosMachine);

            this.pnlCustomerInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCustomerInfo.Height = 45;
            this.pnlCustomerInfo.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);

            this.lblCustomerDetails.Text = "👤 0987654321 • Trần Anh Tuấn\nThành viên CineGold [Giảm 10% F&B + Vé]";
            this.lblCustomerDetails.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCustomerDetails.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.lblCustomerDetails.Location = new System.Drawing.Point(8, 4);
            this.lblCustomerDetails.AutoSize = true;

            this.btnChangeCustomer.Text = "Đổi Khách (F8)";
            this.btnChangeCustomer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnChangeCustomer.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnChangeCustomer.ForeColor = System.Drawing.Color.White;
            this.btnChangeCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeCustomer.Location = new System.Drawing.Point(340, 8);
            this.btnChangeCustomer.Size = new System.Drawing.Size(95, 28);

            this.pnlCustomerInfo.Controls.Add(this.lblCustomerDetails);
            this.pnlCustomerInfo.Controls.Add(this.btnChangeCustomer);

            this.dgvOrderItems.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvOrderItems.Height = 140;
            this.dgvOrderItems.BackgroundColor = System.Drawing.Color.FromArgb(16, 20, 24);
            this.dgvOrderItems.GridColor = System.Drawing.Color.FromArgb(39, 48, 58);
            this.dgvOrderItems.AllowUserToAddRows = false;
            this.dgvOrderItems.ReadOnly = true;
            this.dgvOrderItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrderItems.RowHeadersVisible = false;

            this.colItemName.HeaderText = "DIỄN GIẢI DỊCH VỤ / MẶT HÀNG";
            this.colItemName.Width = 245;
            this.colQty.HeaderText = "SL";
            this.colQty.Width = 35;
            this.colAmount.HeaderText = "THÀNH TIỀN";
            this.colAmount.Width = 115;

            this.dgvOrderItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colItemName, this.colQty, this.colAmount
            });

            this.pnlTotalBlock.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTotalBlock.Height = 78;
            this.pnlTotalBlock.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);
            this.pnlTotalBlock.Padding = new System.Windows.Forms.Padding(8);

            this.lblTotalTitle.Text = "TỔNG CỘNG THANH TOÁN (FINAL TOTAL):";
            this.lblTotalTitle.Font = new System.Drawing.Font("Segoe UI", 6.5F, System.Drawing.FontStyle.Bold);
            this.lblTotalTitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTotalTitle.Location = new System.Drawing.Point(8, 6);
            this.lblTotalTitle.AutoSize = true;

            this.lblTotalAmount.Text = "314.100 đ";
            this.lblTotalAmount.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.lblTotalAmount.Location = new System.Drawing.Point(235, 2);
            this.lblTotalAmount.AutoSize = true;

            this.lblVatInfo.Text = "Đã gồm VAT (8%) • 23.266 đ  |  Tích lũy: +31 pts";
            this.lblVatInfo.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblVatInfo.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblVatInfo.Location = new System.Drawing.Point(8, 52);
            this.lblVatInfo.AutoSize = true;

            this.pnlTotalBlock.Controls.Add(this.lblTotalTitle);
            this.pnlTotalBlock.Controls.Add(this.lblTotalAmount);
            this.pnlTotalBlock.Controls.Add(this.lblVatInfo);

            this.pnlCashQuick.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCashQuick.Height = 100;

            this.lblCashTenderedTitle.Text = "TIỀN KHÁCH ĐƯA:";
            this.lblCashTenderedTitle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCashTenderedTitle.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCashTenderedTitle.Location = new System.Drawing.Point(8, 6);
            this.lblCashTenderedTitle.AutoSize = true;

            this.btnCash200k.Text = "200K";
            this.btnCash200k.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnCash200k.ForeColor = System.Drawing.Color.White;
            this.btnCash200k.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCash200k.Location = new System.Drawing.Point(180, 4);
            this.btnCash200k.Size = new System.Drawing.Size(55, 26);

            this.btnCash500k.Text = "500K";
            this.btnCash500k.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnCash500k.ForeColor = System.Drawing.Color.White;
            this.btnCash500k.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCash500k.Location = new System.Drawing.Point(240, 4);
            this.btnCash500k.Size = new System.Drawing.Size(55, 26);

            this.btnCash1m.Text = "1M";
            this.btnCash1m.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnCash1m.ForeColor = System.Drawing.Color.White;
            this.btnCash1m.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCash1m.Location = new System.Drawing.Point(300, 4);
            this.btnCash1m.Size = new System.Drawing.Size(50, 26);

            this.btnCashExact.Text = "Đủ";
            this.btnCashExact.BackColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.btnCashExact.ForeColor = System.Drawing.Color.Black;
            this.btnCashExact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCashExact.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnCashExact.Location = new System.Drawing.Point(355, 4);
            this.btnCashExact.Size = new System.Drawing.Size(45, 26);

            this.txtCashGiven.Text = "200.000 đ";
            this.txtCashGiven.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Bold);
            this.txtCashGiven.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);
            this.txtCashGiven.ForeColor = System.Drawing.Color.White;
            this.txtCashGiven.Location = new System.Drawing.Point(8, 36);
            this.txtCashGiven.Size = new System.Drawing.Size(190, 26);

            this.lblChangeDue.Text = "TIỀN THỐI LẠI: Không đủ";
            this.lblChangeDue.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Bold);
            this.lblChangeDue.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.lblChangeDue.Location = new System.Drawing.Point(205, 38);
            this.lblChangeDue.AutoSize = true;

            this.pnlCashQuick.Controls.Add(this.lblCashTenderedTitle);
            this.pnlCashQuick.Controls.Add(this.btnCash200k);
            this.pnlCashQuick.Controls.Add(this.btnCash500k);
            this.pnlCashQuick.Controls.Add(this.btnCash1m);
            this.pnlCashQuick.Controls.Add(this.btnCashExact);
            this.pnlCashQuick.Controls.Add(this.txtCashGiven);
            this.pnlCashQuick.Controls.Add(this.lblChangeDue);

            this.pnlPaymentMethods.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPaymentMethods.Height = 48;

            this.btnPayCash.Text = "💵 Tiền mặt [F1]";
            this.btnPayCash.BackColor = System.Drawing.Color.FromArgb(2, 132, 199);
            this.btnPayCash.ForeColor = System.Drawing.Color.White;
            this.btnPayCash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayCash.Location = new System.Drawing.Point(8, 4);
            this.btnPayCash.Size = new System.Drawing.Size(125, 38);

            this.btnPayCard.Text = "💳 Thẻ POS [F2]";
            this.btnPayCard.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnPayCard.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnPayCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayCard.Location = new System.Drawing.Point(140, 4);
            this.btnPayCard.Size = new System.Drawing.Size(125, 38);

            this.btnPayQR.Text = "📱 VietQR [F3]";
            this.btnPayQR.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnPayQR.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnPayQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayQR.Location = new System.Drawing.Point(272, 4);
            this.btnPayQR.Size = new System.Drawing.Size(128, 38);

            this.pnlPaymentMethods.Controls.Add(this.btnPayCash);
            this.pnlPaymentMethods.Controls.Add(this.btnPayCard);
            this.pnlPaymentMethods.Controls.Add(this.btnPayQR);

            this.btnCheckout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCheckout.Height = 52;
            this.btnCheckout.Text = "IN VÉ & HOÀN TẤT GIAO DỊCH [Enter]";
            this.btnCheckout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCheckout.BackColor = System.Drawing.Color.FromArgb(2, 132, 199);
            this.btnCheckout.ForeColor = System.Drawing.Color.White;
            this.btnCheckout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlBottomActionButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReprint.Text = "🖨️ In lại vé [F5]";
            this.btnReprint.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnReprint.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnReprint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReprint.Location = new System.Drawing.Point(8, 8);
            this.btnReprint.Size = new System.Drawing.Size(190, 34);

            this.btnCancelOrder.Text = "❌ Hủy giao dịch [Esc]";
            this.btnCancelOrder.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnCancelOrder.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnCancelOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelOrder.Location = new System.Drawing.Point(205, 8);
            this.btnCancelOrder.Size = new System.Drawing.Size(195, 34);

            this.pnlBottomActionButtons.Controls.Add(this.btnReprint);
            this.pnlBottomActionButtons.Controls.Add(this.btnCancelOrder);

            this.pnlRightTerminal.Controls.Add(this.pnlBottomActionButtons);
            this.pnlRightTerminal.Controls.Add(this.btnCheckout);
            this.pnlRightTerminal.Controls.Add(this.pnlPaymentMethods);
            this.pnlRightTerminal.Controls.Add(this.pnlCashQuick);
            this.pnlRightTerminal.Controls.Add(this.pnlTotalBlock);
            this.pnlRightTerminal.Controls.Add(this.dgvOrderItems);
            this.pnlRightTerminal.Controls.Add(this.pnlCustomerInfo);
            this.pnlRightTerminal.Controls.Add(this.pnlCashierInfo);

            this.pnlMain.Controls.Add(this.pnlRightTerminal);
            this.pnlMain.Controls.Add(this.pnlLeftWork);

            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlTopNav);

            this.pnlTopNav.ResumeLayout(false);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlUserProfile.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlLeftWork.ResumeLayout(false);
            this.pnlMovieBar.ResumeLayout(false);
            this.pnlShowtimeBar.ResumeLayout(false);
            this.pnlTabsBar.ResumeLayout(false);
            this.pnlSeatBottomStatus.ResumeLayout(false);
            this.pnlSeatBottomStatus.PerformLayout();
            this.pnlRightTerminal.ResumeLayout(false);
            this.pnlCashierInfo.ResumeLayout(false);
            this.pnlCashierInfo.PerformLayout();
            this.pnlCustomerInfo.ResumeLayout(false);
            this.pnlCustomerInfo.PerformLayout();
            this.pnlTotalBlock.ResumeLayout(false);
            this.pnlTotalBlock.PerformLayout();
            this.pnlCashQuick.ResumeLayout(false);
            this.pnlCashQuick.PerformLayout();
            this.pnlPaymentMethods.ResumeLayout(false);
            this.pnlBottomActionButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTopNav;
        private System.Windows.Forms.Label lblSubNavMenu;
        private System.Windows.Forms.Label lblShiftInfo;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblNavTitle;
        private System.Windows.Forms.LinkLabel lnkPos;
        private System.Windows.Forms.LinkLabel lnkCombo;
        private System.Windows.Forms.LinkLabel lnkFinance;
        private System.Windows.Forms.LinkLabel lnkOccupancy;
        private System.Windows.Forms.LinkLabel lnkAbout;
        private System.Windows.Forms.Panel pnlUserProfile;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlLeftWork;
        private System.Windows.Forms.Panel pnlMovieBar;
        private System.Windows.Forms.Label lblMovie1;
        private System.Windows.Forms.Label lblMovie2;
        private System.Windows.Forms.Label lblMovie3;
        private System.Windows.Forms.Panel pnlShowtimeBar;
        private System.Windows.Forms.Label lblShowtime1;
        private System.Windows.Forms.Label lblShowtime2;
        private System.Windows.Forms.Panel pnlTabsBar;
        private System.Windows.Forms.Button btnTabSeat;
        private System.Windows.Forms.Button btnTabCombo;
        private System.Windows.Forms.Label lblScreenHeader;
        private System.Windows.Forms.Panel pnlSeatMatrix;
        private System.Windows.Forms.Panel pnlComboCatalog;
        private System.Windows.Forms.Panel pnlSeatBottomStatus;
        private System.Windows.Forms.Label lblSelectedSeats;
        private System.Windows.Forms.Button btnChangeSeat;
        private System.Windows.Forms.Panel pnlRightTerminal;
        private System.Windows.Forms.Panel pnlCashierInfo;
        private System.Windows.Forms.Label lblCashierName;
        private System.Windows.Forms.Label lblPosMachine;
        private System.Windows.Forms.Panel pnlCustomerInfo;
        private System.Windows.Forms.Label lblCustomerDetails;
        private System.Windows.Forms.Button btnChangeCustomer;
        private System.Windows.Forms.DataGridView dgvOrderItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.Panel pnlTotalBlock;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblVatInfo;
        private System.Windows.Forms.Panel pnlCashQuick;
        private System.Windows.Forms.Label lblCashTenderedTitle;
        private System.Windows.Forms.Button btnCash200k;
        private System.Windows.Forms.Button btnCash500k;
        private System.Windows.Forms.Button btnCash1m;
        private System.Windows.Forms.Button btnCashExact;
        private System.Windows.Forms.TextBox txtCashGiven;
        private System.Windows.Forms.Label lblChangeDue;
        private System.Windows.Forms.Panel pnlPaymentMethods;
        private System.Windows.Forms.Button btnPayCash;
        private System.Windows.Forms.Button btnPayCard;
        private System.Windows.Forms.Button btnPayQR;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.Panel pnlBottomActionButtons;
        private System.Windows.Forms.Button btnReprint;
        private System.Windows.Forms.Button btnCancelOrder;
    }
}