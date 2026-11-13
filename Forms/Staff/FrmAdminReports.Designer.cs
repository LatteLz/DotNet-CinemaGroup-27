namespace CinemaManagement.Forms.Admin
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
            this.dgvReports = new System.Windows.Forms.DataGridView();
            this.ColSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColShows = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTotalSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColOccupancy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRevenue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).BeginInit();
            this.SuspendLayout();

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
            this.dgvReports.Size = new System.Drawing.Size(1180, 360);
            this.dgvReports.TabIndex = 0;

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

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 900);
            this.Controls.Add(this.dgvReports);
            this.Name = "FrmAdminReports";
            this.Text = "CINELUXE MANAGEMENT STUDIO - BÁO CÁO TÀI CHÍNH & DOANH THU";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).EndInit();
            this.ResumeLayout(false);
        }

        public System.Windows.Forms.Panel pnlSidebar;
        public System.Windows.Forms.Label lblLogo;
        public System.Windows.Forms.LinkLabel lnkPos;
        public System.Windows.Forms.LinkLabel lnkCombo;
        public System.Windows.Forms.LinkLabel lnkReports;
        public System.Windows.Forms.LinkLabel lnkOccupancy;
        public System.Windows.Forms.LinkLabel lnkAbout;
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