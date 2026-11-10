namespace CinemaManagement.Forms.Staff
{
    partial class FrmAdminCombos
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
            this.lblSysTitle = new System.Windows.Forms.Label();
            this.pnlSubNav = new System.Windows.Forms.Panel();
            this.btnAddComboTop = new System.Windows.Forms.Button();
            this.btnUpdatePrice = new System.Windows.Forms.Button();
            this.btnImportStore = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlFilterBar = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvCombos = new System.Windows.Forms.DataGridView();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.lblDetailHeader = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblFooterStatus = new System.Windows.Forms.Label();

            this.colSku = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.colNameDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlSubNav.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCombos)).BeginInit();
            this.pnlDetails.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            this.ClientSize = new System.Drawing.Size(1440, 900);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlSubNav);
            this.Controls.Add(this.pnlTopNav);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "FrmAdminCombos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CineLuxe Management - Quản Lý Kho Combo Bắp Nước";

            this.pnlTopNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopNav.Height = 36;
            this.pnlTopNav.BackColor = System.Drawing.Color.FromArgb(11, 15, 18);

            this.pnlSubNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubNav.Height = 56;
            this.pnlSubNav.BackColor = System.Drawing.Color.FromArgb(20, 24, 29);

            this.btnAddComboTop.Text = "➕ Thêm Combo Mới (Ctrl+N)";
            this.btnAddComboTop.BackColor = System.Drawing.Color.FromArgb(217, 119, 6);
            this.btnAddComboTop.ForeColor = System.Drawing.Color.White;
            this.btnAddComboTop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddComboTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddComboTop.Location = new System.Drawing.Point(16, 10);
            this.btnAddComboTop.Size = new System.Drawing.Size(210, 36);

            this.btnUpdatePrice.Text = "🏷️ Cập nhật Giá sỉ/lẻ";
            this.btnUpdatePrice.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnUpdatePrice.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnUpdatePrice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdatePrice.Location = new System.Drawing.Point(236, 10);
            this.btnUpdatePrice.Size = new System.Drawing.Size(170, 36);

            this.btnImportStore.Text = "📦 Nhập kho F&B";
            this.btnImportStore.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnImportStore.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnImportStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportStore.Location = new System.Drawing.Point(414, 10);
            this.btnImportStore.Size = new System.Drawing.Size(150, 36);

            this.btnExportExcel.Text = "📊 Xuất Excel";
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnExportExcel.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Location = new System.Drawing.Point(572, 10);
            this.btnExportExcel.Size = new System.Drawing.Size(130, 36);

            this.pnlSubNav.Controls.Add(this.btnAddComboTop);
            this.pnlSubNav.Controls.Add(this.btnUpdatePrice);
            this.pnlSubNav.Controls.Add(this.btnImportStore);
            this.pnlSubNav.Controls.Add(this.btnExportExcel);

            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 260;
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(20, 24, 29);

            this.lblLogo.Text = "CINELUXE MANAGEMENT";
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(56, 189, 248);
            this.lblLogo.Location = new System.Drawing.Point(16, 20);
            this.lblLogo.AutoSize = true;
            this.pnlSidebar.Controls.Add(this.lblLogo);

            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(16, 20, 24);
            this.pnlMain.Padding = new System.Windows.Forms.Padding(16);

            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterBar.Height = 48;

            this.txtSearch.Text = "🔍 Tìm kiếm SKU, tên combo...";
            this.txtSearch.Width = 300;
            this.txtSearch.Height = 30;
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(11, 15, 18);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.pnlFilterBar.Controls.Add(this.txtSearch);

            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetails.Width = 320;
            this.pnlDetails.BackColor = System.Drawing.Color.FromArgb(24, 28, 35);
            this.pnlDetails.Padding = new System.Windows.Forms.Padding(16);

            this.lblDetailHeader.Text = "⚙️ CHI TIẾT MẶT HÀNG F&B";
            this.lblDetailHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetailHeader.ForeColor = System.Drawing.Color.White;
            this.lblDetailHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetailHeader.Height = 35;

            this.btnSave.Text = "💾 Lưu thay đổi (Ctrl+S)";
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.Height = 40;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(2, 132, 199);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlDetails.Controls.Add(this.btnSave);
            this.pnlDetails.Controls.Add(this.lblDetailHeader);

            this.dgvCombos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCombos.AllowUserToAddRows = false;
            this.dgvCombos.ReadOnly = true;
            this.dgvCombos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCombos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCombos.ColumnHeadersHeight = 40;

            this.colSku.HeaderText = "MÃ SKU"; 
            this.colSku.Width = 90;
            this.colImg.HeaderText = "ẢNH"; 
            this.colImg.Width = 50;
            this.colNameDesc.HeaderText = "TÊN MÓN / COMBO"; 
            this.colNameDesc.Width = 200;
            this.colCategory.HeaderText = "PHÂN LOẠI"; 
            this.colCategory.Width = 120;
            this.colCost.HeaderText = "GIÁ VỐN"; 
            this.colCost.Width = 85;
            this.colPrice.HeaderText = "GIÁ NIÊM YẾT"; 
            this.colPrice.Width = 95;
            this.colStock.HeaderText = "TỒN"; 
            this.colStock.Width = 55;

            this.dgvCombos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colSku, this.colImg, this.colNameDesc, this.colCategory, this.colCost, this.colPrice, this.colStock
            });

            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 35;
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(11, 15, 18);

            this.lblFooterStatus.Text = "🟢 Tổng mặt hàng: 18 sản phẩm F&B  |  Giá trị tồn kho: 42.500.000 đ  |  SQLite WAL Mode: OK";
            this.lblFooterStatus.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.lblFooterStatus.ForeColor = System.Drawing.Color.FromArgb(74, 222, 128);
            this.lblFooterStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooter.Controls.Add(this.lblFooterStatus);

            this.pnlMain.Controls.Add(this.dgvCombos);
            this.pnlMain.Controls.Add(this.pnlDetails);
            this.pnlMain.Controls.Add(this.pnlFilterBar);
            this.pnlMain.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlSubNav);
            this.Controls.Add(this.pnlTopNav);

            this.pnlSubNav.ResumeLayout(false);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCombos)).EndInit();
            this.pnlDetails.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTopNav;
        private System.Windows.Forms.Label lblSysTitle;
        private System.Windows.Forms.Panel pnlSubNav;
        private System.Windows.Forms.Button btnAddComboTop;
        private System.Windows.Forms.Button btnUpdatePrice;
        private System.Windows.Forms.Button btnImportStore;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlFilterBar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvCombos;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblDetailHeader;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblFooterStatus;

        private System.Windows.Forms.DataGridViewTextBoxColumn colSku;
        private System.Windows.Forms.DataGridViewImageColumn colImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
    }
}