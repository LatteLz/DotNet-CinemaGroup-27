namespace CinemaManagement.Forms.Staff
{
    partial class FrmAdminReports
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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.lnkOccupancy = new System.Windows.Forms.LinkLabel();
            this.lnkReports = new System.Windows.Forms.LinkLabel();
            this.lnkUsers = new System.Windows.Forms.LinkLabel();
            this.lnkCombo = new System.Windows.Forms.LinkLabel();
            this.lnkPos = new System.Windows.Forms.LinkLabel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblShift = new System.Windows.Forms.Label();
            this.lblBreadcrumb = new System.Windows.Forms.Label();
            this.pnlFilterBar = new System.Windows.Forms.Panel();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.btnMonthThis = new System.Windows.Forms.Button();
            this.pnlKpiContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlChartContainer = new System.Windows.Forms.Panel();
            this.pnlDonutChart = new System.Windows.Forms.Panel();
            this.lblDonutTitle = new System.Windows.Forms.Label();
            this.pnlBarChart = new System.Windows.Forms.Panel();
            this.lblBarTitle = new System.Windows.Forms.Label();
            this.dgvReports = new System.Windows.Forms.DataGridView();
            this.ColSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColShows = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTotalSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColOccupancy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRevenue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblFooterStatus = new System.Windows.Forms.Label();
            this.pnlSidebar.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            this.pnlChartContainer.SuspendLayout();
            this.pnlDonutChart.SuspendLayout();
            this.pnlBarChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).BeginInit();
            this.SuspendLayout();

            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlSidebar.Controls.Add(this.lblUser);
            this.pnlSidebar.Controls.Add(this.lnkOccupancy);
            this.pnlSidebar.Controls.Add(this.lnkReports);
            this.pnlSidebar.Controls.Add(this.lnkUsers);
            this.pnlSidebar.Controls.Add(this.lnkCombo);
            this.pnlSidebar.Controls.Add(this.lnkPos);
            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(220, 900);
            this.pnlSidebar.TabIndex = 0;

            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblUser.Location = new System.Drawing.Point(20, 815);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(107, 34);
            this.lblUser.TabIndex = 7;
            this.lblUser.Text = "Julian Vance\r\nQuản Lý Trưởng";

            this.lnkOccupancy.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(211)))), ((int)(((byte)(252)))));
            this.lnkOccupancy.AutoSize = true;
            this.lnkOccupancy.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lnkOccupancy.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkOccupancy.Location = new System.Drawing.Point(20, 280);
            this.lnkOccupancy.Name = "lnkOccupancy";
            this.lnkOccupancy.Size = new System.Drawing.Size(156, 19);
            this.lnkOccupancy.TabIndex = 5;
            this.lnkOccupancy.TabStop = true;
            this.lnkOccupancy.Text = "Lấp đầy Khán Phòng";

            this.lnkReports.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(211)))), ((int)(((byte)(252)))));
            this.lnkReports.AutoSize = true;
            this.lnkReports.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lnkReports.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkReports.Location = new System.Drawing.Point(20, 230);
            this.lnkReports.Name = "lnkReports";
            this.lnkReports.Size = new System.Drawing.Size(162, 19);
            this.lnkReports.TabIndex = 4;
            this.lnkReports.TabStop = true;
            this.lnkReports.Text = "Tài Chính & Doanh Thu";

            this.lnkUsers.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(211)))), ((int)(((byte)(252)))));
            this.lnkUsers.AutoSize = true;
            this.lnkUsers.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lnkUsers.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkUsers.Location = new System.Drawing.Point(20, 180);
            this.lnkUsers.Name = "lnkUsers";
            this.lnkUsers.Size = new System.Drawing.Size(155, 19);
            this.lnkUsers.TabIndex = 3;
            this.lnkUsers.TabStop = true;
            this.lnkUsers.Text = "Quản Lý Nhân Viên";

            this.lnkCombo.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(211)))), ((int)(((byte)(252)))));
            this.lnkCombo.AutoSize = true;
            this.lnkCombo.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lnkCombo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkCombo.Location = new System.Drawing.Point(20, 130);
            this.lnkCombo.Name = "lnkCombo";
            this.lnkCombo.Size = new System.Drawing.Size(151, 19);
            this.lnkCombo.TabIndex = 2;
            this.lnkCombo.TabStop = true;
            this.lnkCombo.Text = "Quản Lý Combo F&B";

            this.lnkPos.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(211)))), ((int)(((byte)(252)))));
            this.lnkPos.AutoSize = true;
            this.lnkPos.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lnkPos.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkPos.Location = new System.Drawing.Point(20, 80);
            this.lnkPos.Name = "lnkPos";
            this.lnkPos.Size = new System.Drawing.Size(130, 19);
            this.lnkPos.TabIndex = 1;
            this.lnkPos.TabStop = true;
            this.lnkPos.Text = "Bán Vé Quầy POS";

            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(20, 20);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(95, 25);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "CINELUXE";

            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlHeader.Controls.Add(this.btnHelp);
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Controls.Add(this.lblShift);
            this.pnlHeader.Controls.Add(this.lblBreadcrumb);
            this.pnlHeader.Location = new System.Drawing.Point(220, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1220, 48);
            this.pnlHeader.TabIndex = 1;

            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnHelp.ForeColor = System.Drawing.Color.White;
            this.btnHelp.Location = new System.Drawing.Point(1030, 10);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(80, 28);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = false;

            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(940, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 28);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;

            this.lblShift.AutoSize = true;
            this.lblShift.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.lblShift.Location = new System.Drawing.Point(780, 15);
            this.lblShift.Name = "lblShift";
            this.lblShift.Size = new System.Drawing.Size(125, 15);
            this.lblShift.TabIndex = 1;
            this.lblShift.Text = "SHIFT: MORNING (A)";

            this.lblBreadcrumb.AutoSize = true;
            this.lblBreadcrumb.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBreadcrumb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblBreadcrumb.Location = new System.Drawing.Point(20, 15);
            this.lblBreadcrumb.Name = "lblBreadcrumb";
            this.lblBreadcrumb.Size = new System.Drawing.Size(315, 15);
            this.lblBreadcrumb.TabIndex = 0;
            this.lblBreadcrumb.Text = "BÁN VÉ POS / KHO COMBO / BÁO CÁO DOANH THU";

            this.pnlFilterBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.pnlFilterBar.Controls.Add(this.btnExportExcel);
            this.pnlFilterBar.Controls.Add(this.btnQuery);
            this.pnlFilterBar.Controls.Add(this.btnMonthThis);
            this.pnlFilterBar.Location = new System.Drawing.Point(240, 60);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Size = new System.Drawing.Size(1180, 65);
            this.pnlFilterBar.TabIndex = 2;

            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(1030, 15);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(120, 35);
            this.btnExportExcel.TabIndex = 2;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;

            this.btnQuery.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuery.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuery.ForeColor = System.Drawing.Color.White;
            this.btnQuery.Location = new System.Drawing.Point(900, 15);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(120, 35);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "Truy Vấn (F5)";
            this.btnQuery.UseVisualStyleBackColor = false;

            this.btnMonthThis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.btnMonthThis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMonthThis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMonthThis.ForeColor = System.Drawing.Color.White;
            this.btnMonthThis.Location = new System.Drawing.Point(20, 15);
            this.btnMonthThis.Name = "btnMonthThis";
            this.btnMonthThis.Size = new System.Drawing.Size(140, 35);
            this.btnMonthThis.TabIndex = 0;
            this.btnMonthThis.Text = "Tháng Này [Active]";
            this.btnMonthThis.UseVisualStyleBackColor = false;

            this.pnlKpiContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(18)))));
            this.pnlKpiContainer.Location = new System.Drawing.Point(240, 135);
            this.pnlKpiContainer.Name = "pnlKpiContainer";
            this.pnlKpiContainer.Size = new System.Drawing.Size(1180, 100);
            this.pnlKpiContainer.TabIndex = 3;

            this.pnlChartContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(18)))));
            this.pnlChartContainer.Controls.Add(this.pnlDonutChart);
            this.pnlChartContainer.Controls.Add(this.pnlBarChart);
            this.pnlChartContainer.Location = new System.Drawing.Point(240, 245);
            this.pnlChartContainer.Name = "pnlChartContainer";
            this.pnlChartContainer.Size = new System.Drawing.Size(1180, 210);
            this.pnlChartContainer.TabIndex = 4;

            this.pnlDonutChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.pnlDonutChart.Controls.Add(this.lblDonutTitle);
            this.pnlDonutChart.Location = new System.Drawing.Point(780, 0);
            this.pnlDonutChart.Name = "pnlDonutChart";
            this.pnlDonutChart.Size = new System.Drawing.Size(400, 210);
            this.pnlDonutChart.TabIndex = 1;

            this.lblDonutTitle.AutoSize = true;
            this.lblDonutTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDonutTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblDonutTitle.Location = new System.Drawing.Point(15, 12);
            this.lblDonutTitle.Name = "lblDonutTitle";
            this.lblDonutTitle.Size = new System.Drawing.Size(209, 15);
            this.lblDonutTitle.TabIndex = 0;
            this.lblDonutTitle.Text = "TỶ TRỌNG KÊNH BÁN TOUCHPOINT";

            this.pnlBarChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.pnlBarChart.Controls.Add(this.lblBarTitle);
            this.pnlBarChart.Location = new System.Drawing.Point(0, 0);
            this.pnlBarChart.Name = "pnlBarChart";
            this.pnlBarChart.Size = new System.Drawing.Size(765, 210);
            this.pnlBarChart.TabIndex = 0;

            this.lblBarTitle.AutoSize = true;
            this.lblBarTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBarTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblBarTitle.Location = new System.Drawing.Point(15, 12);
            this.lblBarTitle.Name = "lblBarTitle";
            this.lblBarTitle.Size = new System.Drawing.Size(262, 15);
            this.lblBarTitle.TabIndex = 0;
            this.lblBarTitle.Text = "XU HƯỚNG DOANH THU NGÀY TRONG THÁNG";

            this.dgvReports.AllowUserToAddRows = false;
            this.dgvReports.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReports.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReports.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSTT,
            this.ColName,
            this.ColCategory,
            this.ColShows,
            this.ColTotalSold,
            this.ColOccupancy,
            this.ColRevenue,
            this.ColRatio});
            this.dgvReports.Location = new System.Drawing.Point(240, 465);
            this.dgvReports.Name = "dgvReports";
            this.dgvReports.RowHeadersVisible = false;
            this.dgvReports.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReports.Size = new System.Drawing.Size(1180, 360);
            this.dgvReports.TabIndex = 5;

            this.ColSTT.HeaderText = "#";
            this.ColSTT.Name = "ColSTT";

            this.ColName.HeaderText = "TÊN PHIM / COMBO BẮP NƯỚC";
            this.ColName.Name = "ColName";

            this.ColCategory.HeaderText = "PHÂN LOẠI";
            this.ColCategory.Name = "ColCategory";

            this.ColShows.HeaderText = "SUẤT CHIẾU / THÔNG TIN";
            this.ColShows.Name = "ColShows";

            this.ColTotalSold.HeaderText = "TỔNG VÉ";
            this.ColTotalSold.Name = "ColTotalSold";

            this.ColOccupancy.HeaderText = "LẤP ĐẦY TB";
            this.ColOccupancy.Name = "ColOccupancy";

            this.ColRevenue.HeaderText = "DOANH THU (VND)";
            this.ColRevenue.Name = "ColRevenue";

            this.ColRatio.HeaderText = "THỜI GIAN GIAO DỊCH";
            this.ColRatio.Name = "ColRatio";

            this.lblFooterStatus.AutoSize = true;
            this.lblFooterStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblFooterStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblFooterStatus.Location = new System.Drawing.Point(240, 835);
            this.lblFooterStatus.Name = "lblFooterStatus";
            this.lblFooterStatus.Size = new System.Drawing.Size(465, 13);
            this.lblFooterStatus.TabIndex = 6;
            this.lblFooterStatus.Text = "Truy vấn LINQ: 18ms QUA SQLITE INDEX • MÃ BÁO CÁO: REP-20241026-FIN-091 • Ký số: MD5-VERIFIED";

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1440, 900);
            this.Controls.Add(this.lblFooterStatus);
            this.Controls.Add(this.dgvReports);
            this.Controls.Add(this.pnlChartContainer);
            this.Controls.Add(this.pnlKpiContainer);
            this.Controls.Add(this.pnlFilterBar);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmAdminReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CINELUXE MANAGEMENT STUDIO - BÁO CÁO TÀI CHÍNH & DOANH THU";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlChartContainer.ResumeLayout(false);
            this.pnlDonutChart.ResumeLayout(false);
            this.pnlDonutChart.PerformLayout();
            this.pnlBarChart.ResumeLayout(false);
            this.pnlBarChart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public System.Windows.Forms.Panel pnlSidebar;
        public System.Windows.Forms.Label lblLogo;
        public System.Windows.Forms.LinkLabel lnkPos;
        public System.Windows.Forms.LinkLabel lnkCombo;
        public System.Windows.Forms.LinkLabel lnkUsers;
        public System.Windows.Forms.LinkLabel lnkReports;
        public System.Windows.Forms.LinkLabel lnkOccupancy;
        public System.Windows.Forms.Label lblUser;
        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblBreadcrumb;
        public System.Windows.Forms.Label lblShift;
        public System.Windows.Forms.Button btnRefresh;
        public System.Windows.Forms.Button btnHelp;
        public System.Windows.Forms.Panel pnlFilterBar;
        public System.Windows.Forms.Button btnMonthThis;
        public System.Windows.Forms.Button btnQuery;
        public System.Windows.Forms.Button btnExportExcel;
        public System.Windows.Forms.FlowLayoutPanel pnlKpiContainer;
        public System.Windows.Forms.Panel pnlChartContainer;
        public System.Windows.Forms.Panel pnlBarChart;
        public System.Windows.Forms.Label lblBarTitle;
        public System.Windows.Forms.Panel pnlDonutChart;
        public System.Windows.Forms.Label lblDonutTitle;
        public System.Windows.Forms.DataGridView dgvReports;
        public System.Windows.Forms.DataGridViewTextBoxColumn ColSTT;
        public System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        public System.Windows.Forms.DataGridViewTextBoxColumn ColCategory;
        public System.Windows.Forms.DataGridViewTextBoxColumn ColShows;
        public System.Windows.Forms.DataGridViewTextBoxColumn ColTotalSold;
        public System.Windows.Forms.DataGridViewTextBoxColumn ColOccupancy;
        public System.Windows.Forms.DataGridViewTextBoxColumn ColRevenue;
        public System.Windows.Forms.DataGridViewTextBoxColumn ColRatio;
        public System.Windows.Forms.Label lblFooterStatus;
    }
}