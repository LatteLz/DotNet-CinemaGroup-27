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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lnkUsers = new System.Windows.Forms.LinkLabel();
            this.lnkCombo = new System.Windows.Forms.LinkLabel();
            this.lnkPos = new System.Windows.Forms.LinkLabel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.btnAddComboTop = new System.Windows.Forms.Button();
            this.btnUpdatePrice = new System.Windows.Forms.Button();
            this.btnImportStore = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvCombos = new System.Windows.Forms.DataGridView();
            this.colSku = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colListPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblFooterStatus = new System.Windows.Forms.Label();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCombos)).BeginInit();
            this.SuspendLayout();

            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlSidebar.Controls.Add(this.lnkUsers);
            this.pnlSidebar.Controls.Add(this.lnkCombo);
            this.pnlSidebar.Controls.Add(this.lnkPos);
            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(220, 720);
            this.pnlSidebar.TabIndex = 0;

            this.lnkUsers.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkUsers.AutoSize = true;
            this.lnkUsers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkUsers.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkUsers.Location = new System.Drawing.Point(20, 190);
            this.lnkUsers.Name = "lnkUsers";
            this.lnkUsers.Size = new System.Drawing.Size(127, 19);
            this.lnkUsers.TabIndex = 3;
            this.lnkUsers.TabStop = true;
            this.lnkUsers.Text = "Quản Lý Nhân Viên";

            this.lnkCombo.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkCombo.AutoSize = true;
            this.lnkCombo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkCombo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkCombo.Location = new System.Drawing.Point(20, 135);
            this.lnkCombo.Name = "lnkCombo";
            this.lnkCombo.Size = new System.Drawing.Size(120, 19);
            this.lnkCombo.TabIndex = 2;
            this.lnkCombo.TabStop = true;
            this.lnkCombo.Text = "Quản Lý Combo FB";

            this.lnkPos.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkPos.AutoSize = true;
            this.lnkPos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkPos.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkPos.Location = new System.Drawing.Point(20, 80);
            this.lnkPos.Name = "lnkPos";
            this.lnkPos.Size = new System.Drawing.Size(120, 19);
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

            this.btnAddComboTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.btnAddComboTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddComboTop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddComboTop.ForeColor = System.Drawing.Color.Black;
            this.btnAddComboTop.Location = new System.Drawing.Point(240, 15);
            this.btnAddComboTop.Name = "btnAddComboTop";
            this.btnAddComboTop.Size = new System.Drawing.Size(125, 30);
            this.btnAddComboTop.TabIndex = 1;
            this.btnAddComboTop.Text = "+ Thêm Combo Mới";
            this.btnAddComboTop.UseVisualStyleBackColor = false;

            this.btnUpdatePrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnUpdatePrice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdatePrice.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnUpdatePrice.ForeColor = System.Drawing.Color.White;
            this.btnUpdatePrice.Location = new System.Drawing.Point(375, 15);
            this.btnUpdatePrice.Name = "btnUpdatePrice";
            this.btnUpdatePrice.Size = new System.Drawing.Size(120, 30);
            this.btnUpdatePrice.TabIndex = 2;
            this.btnUpdatePrice.Text = "Cập nhật Giá sỉ/lẻ";
            this.btnUpdatePrice.UseVisualStyleBackColor = false;

            this.btnImportStore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnImportStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportStore.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnImportStore.ForeColor = System.Drawing.Color.White;
            this.btnImportStore.Location = new System.Drawing.Point(505, 15);
            this.btnImportStore.Name = "btnImportStore";
            this.btnImportStore.Size = new System.Drawing.Size(100, 30);
            this.btnImportStore.TabIndex = 3;
            this.btnImportStore.Text = "Nhập kho FB";
            this.btnImportStore.UseVisualStyleBackColor = false;

            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(615, 15);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(95, 30);
            this.btnExportExcel.TabIndex = 4;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;

            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtSearch.Location = new System.Drawing.Point(240, 65);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 24);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.Text = "🔍 Tìm kiếm SKU, tên combo...";

            this.dgvCombos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCombos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSku,
            this.colImage,
            this.colNameDesc,
            this.colCategory,
            this.colCostPrice,
            this.colListPrice,
            this.colStock});
            this.dgvCombos.Location = new System.Drawing.Point(240, 105);
            this.dgvCombos.Name = "dgvCombos";
            this.dgvCombos.RowHeadersVisible = false;
            this.dgvCombos.Size = new System.Drawing.Size(1010, 550);
            this.dgvCombos.TabIndex = 6;

            this.colSku.HeaderText = "MÃ SKU";
            this.colSku.Name = "colSku";
            this.colSku.Width = 110;

            this.colImage.HeaderText = "ẢNH";
            this.colImage.Name = "colImage";
            this.colImage.Width = 60;

            this.colNameDesc.HeaderText = "TÊN MÓN / COMBO";
            this.colNameDesc.Name = "colNameDesc";
            this.colNameDesc.Width = 300;

            this.colCategory.HeaderText = "PHÂN LOẠI";
            this.colCategory.Name = "colCategory";
            this.colCategory.Width = 150;

            this.colCostPrice.HeaderText = "GIÁ VỐN";
            this.colCostPrice.Name = "colCostPrice";
            this.colCostPrice.Width = 120;

            this.colListPrice.HeaderText = "GIÁ NIÊM YẾT";
            this.colListPrice.Name = "colListPrice";
            this.colListPrice.Width = 130;

            this.colStock.HeaderText = "TỒN";
            this.colStock.Name = "colStock";
            this.colStock.Width = 110;

            this.lblFooterStatus.AutoSize = true;
            this.lblFooterStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFooterStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblFooterStatus.Location = new System.Drawing.Point(240, 675);
            this.lblFooterStatus.Name = "lblFooterStatus";
            this.lblFooterStatus.Size = new System.Drawing.Size(0, 15);
            this.lblFooterStatus.TabIndex = 7;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.lblFooterStatus);
            this.Controls.Add(this.dgvCombos);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnImportStore);
            this.Controls.Add(this.btnUpdatePrice);
            this.Controls.Add(this.btnAddComboTop);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmAdminCombos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cinéluxe Management - Quản lý Kho Combo Bắp Nước";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCombos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.LinkLabel lnkPos;
        private System.Windows.Forms.LinkLabel lnkCombo;
        private System.Windows.Forms.LinkLabel lnkUsers;
        private System.Windows.Forms.Button btnAddComboTop;
        private System.Windows.Forms.Button btnUpdatePrice;
        private System.Windows.Forms.Button btnImportStore;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvCombos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSku;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colListPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
        private System.Windows.Forms.Label lblFooterStatus;
    }
}