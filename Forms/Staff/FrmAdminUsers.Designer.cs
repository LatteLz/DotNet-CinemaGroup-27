namespace CinemaManagement.Forms.Staff
{
    partial class FrmAdminUsers
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
            this.lnkOccupancy = new System.Windows.Forms.LinkLabel();
            this.lnkReports = new System.Windows.Forms.LinkLabel();
            this.lnkUsers = new System.Windows.Forms.LinkLabel();
            this.lnkCombo = new System.Windows.Forms.LinkLabel();
            this.lnkPos = new System.Windows.Forms.LinkLabel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlCard1 = new System.Windows.Forms.Panel();
            this.lblTotalAccounts = new System.Windows.Forms.Label();
            this.lblCardTitle1 = new System.Windows.Forms.Label();
            this.pnlCard2 = new System.Windows.Forms.Panel();
            this.lblAdminCount = new System.Windows.Forms.Label();
            this.lblCardTitle2 = new System.Windows.Forms.Label();
            this.pnlCard3 = new System.Windows.Forms.Panel();
            this.lblStaffCount = new System.Windows.Forms.Label();
            this.lblCardTitle3 = new System.Windows.Forms.Label();
            this.pnlCard4 = new System.Windows.Forms.Panel();
            this.lblCustomerCount = new System.Windows.Forms.Label();
            this.lblCardTitle4 = new System.Windows.Forms.Label();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSidebar.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlCard1.SuspendLayout();
            this.pnlCard2.SuspendLayout();
            this.pnlCard3.SuspendLayout();
            this.pnlCard4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();

            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlSidebar.Controls.Add(this.lnkOccupancy);
            this.pnlSidebar.Controls.Add(this.lnkReports);
            this.pnlSidebar.Controls.Add(this.lnkUsers);
            this.pnlSidebar.Controls.Add(this.lnkCombo);
            this.pnlSidebar.Controls.Add(this.lnkPos);
            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(220, 720);
            this.pnlSidebar.TabIndex = 0;

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
            this.lnkCombo.Text = "Quản Lý Combo FB";

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

            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(18)))));
            this.pnlHeader.Controls.Add(this.btnAddUser);
            this.pnlHeader.Controls.Add(this.txtSearch);
            this.pnlHeader.Controls.Add(this.lblSubTitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Location = new System.Drawing.Point(240, 20);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1010, 95);
            this.pnlHeader.TabIndex = 1;

            this.btnAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddUser.ForeColor = System.Drawing.Color.Black;
            this.btnAddUser.Location = new System.Drawing.Point(885, 50);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(115, 30);
            this.btnAddUser.TabIndex = 3;
            this.btnAddUser.Text = "+ Thêm Người";
            this.btnAddUser.UseVisualStyleBackColor = false;

            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtSearch.Location = new System.Drawing.Point(0, 56);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(350, 24);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.Text = "🔍 Tìm kiếm theo tên, email, username hoặc SĐT...";

            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lblSubTitle.Location = new System.Drawing.Point(0, 32);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(217, 15);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "BẢO MẬT PHÂN QUYỀN RBAC-LEVEL 4 [ACTIVE]";

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(215, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ NGƯỜI DÙNG";

            this.pnlCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlCard1.Controls.Add(this.lblTotalAccounts);
            this.pnlCard1.Controls.Add(this.lblCardTitle1);
            this.pnlCard1.Location = new System.Drawing.Point(240, 130);
            this.pnlCard1.Name = "pnlCard1";
            this.pnlCard1.Size = new System.Drawing.Size(235, 75);
            this.pnlCard1.TabIndex = 2;

            this.lblTotalAccounts.AutoSize = true;
            this.lblTotalAccounts.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotalAccounts.ForeColor = System.Drawing.Color.White;
            this.lblTotalAccounts.Location = new System.Drawing.Point(15, 30);
            this.lblTotalAccounts.Name = "lblTotalAccounts";
            this.lblTotalAccounts.Size = new System.Drawing.Size(25, 30);
            this.lblTotalAccounts.TabIndex = 1;
            this.lblTotalAccounts.Text = "2";

            this.lblCardTitle1.AutoSize = true;
            this.lblCardTitle1.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCardTitle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCardTitle1.Location = new System.Drawing.Point(15, 10);
            this.lblCardTitle1.Name = "lblCardTitle1";
            this.lblCardTitle1.Size = new System.Drawing.Size(91, 15);
            this.lblCardTitle1.TabIndex = 0;
            this.lblCardTitle1.Text = "TỔNG TÀI KHOẢN";

            this.pnlCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlCard2.Controls.Add(this.lblAdminCount);
            this.pnlCard2.Controls.Add(this.lblCardTitle2);
            this.pnlCard2.Location = new System.Drawing.Point(490, 130);
            this.pnlCard2.Name = "pnlCard2";
            this.pnlCard2.Size = new System.Drawing.Size(235, 75);
            this.pnlCard2.TabIndex = 3;

            this.lblAdminCount.AutoSize = true;
            this.lblAdminCount.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAdminCount.ForeColor = System.Drawing.Color.White;
            this.lblAdminCount.Location = new System.Drawing.Point(15, 30);
            this.lblAdminCount.Name = "lblAdminCount";
            this.lblAdminCount.Size = new System.Drawing.Size(25, 30);
            this.lblAdminCount.TabIndex = 1;
            this.lblAdminCount.Text = "1";

            this.lblCardTitle2.AutoSize = true;
            this.lblCardTitle2.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCardTitle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCardTitle2.Location = new System.Drawing.Point(15, 10);
            this.lblCardTitle2.Name = "lblCardTitle2";
            this.lblCardTitle2.Size = new System.Drawing.Size(127, 15);
            this.lblCardTitle2.TabIndex = 0;
            this.lblCardTitle2.Text = "QUẢN TRỊ VIÊN (ADMIN)";

            this.pnlCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlCard3.Controls.Add(this.lblStaffCount);
            this.pnlCard3.Controls.Add(this.lblCardTitle3);
            this.pnlCard3.Location = new System.Drawing.Point(740, 130);
            this.pnlCard3.Name = "pnlCard3";
            this.pnlCard3.Size = new System.Drawing.Size(235, 75);
            this.pnlCard3.TabIndex = 4;

            this.lblStaffCount.AutoSize = true;
            this.lblStaffCount.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblStaffCount.ForeColor = System.Drawing.Color.White;
            this.lblStaffCount.Location = new System.Drawing.Point(15, 30);
            this.lblStaffCount.Name = "lblStaffCount";
            this.lblStaffCount.Size = new System.Drawing.Size(25, 30);
            this.lblStaffCount.TabIndex = 1;
            this.lblStaffCount.Text = "1";

            this.lblCardTitle3.AutoSize = true;
            this.lblCardTitle3.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCardTitle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCardTitle3.Location = new System.Drawing.Point(15, 10);
            this.lblCardTitle3.Name = "lblCardTitle3";
            this.lblCardTitle3.Size = new System.Drawing.Size(133, 15);
            this.lblCardTitle3.TabIndex = 0;
            this.lblCardTitle3.Text = "NHÂN VIÊN TRỰC CA (STAFF)";

            this.pnlCard4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlCard4.Controls.Add(this.lblCustomerCount);
            this.pnlCard4.Controls.Add(this.lblCardTitle4);
            this.pnlCard4.Location = new System.Drawing.Point(990, 130);
            this.pnlCard4.Name = "pnlCard4";
            this.pnlCard4.Size = new System.Drawing.Size(260, 75);
            this.pnlCard4.TabIndex = 5;

            this.lblCustomerCount.AutoSize = true;
            this.lblCustomerCount.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCustomerCount.ForeColor = System.Drawing.Color.White;
            this.lblCustomerCount.Location = new System.Drawing.Point(15, 30);
            this.lblCustomerCount.Name = "lblCustomerCount";
            this.lblCustomerCount.Size = new System.Drawing.Size(25, 30);
            this.lblCustomerCount.TabIndex = 1;
            this.lblCustomerCount.Text = "0";

            this.lblCardTitle4.AutoSize = true;
            this.lblCardTitle4.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCardTitle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCardTitle4.Location = new System.Drawing.Point(15, 10);
            this.lblCardTitle4.Name = "lblCardTitle4";
            this.lblCardTitle4.Size = new System.Drawing.Size(110, 15);
            this.lblCardTitle4.TabIndex = 0;
            this.lblCardTitle4.Text = "HỘI VIÊN (CUSTOMERS)";

            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colUsername,
            this.colEmail,
            this.colPhone,
            this.colRole,
            this.colStatus,
            this.colCreatedAt,
            this.colAction});
            this.dgvUsers.Location = new System.Drawing.Point(240, 220);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.Size = new System.Drawing.Size(1010, 470);
            this.dgvUsers.TabIndex = 6;

            this.colId.HeaderText = "MÃ ID";
            this.colId.Name = "colId";
            this.colId.Width = 90;

            this.colUsername.HeaderText = "NGƯỜI DÙNG / TÀI KHOẢN";
            this.colUsername.Name = "colUsername";
            this.colUsername.Width = 180;

            this.colEmail.HeaderText = "LIÊN HỆ EMAIL";
            this.colEmail.Name = "colEmail";
            this.colEmail.Width = 190;

            this.colPhone.HeaderText = "SỐ ĐIỆN THOẠI";
            this.colPhone.Name = "colPhone";
            this.colPhone.Width = 120;

            this.colRole.HeaderText = "VAI TRÒ / NHIỆM VỤ";
            this.colRole.Name = "colRole";
            this.colRole.Width = 130;

            this.colStatus.HeaderText = "TRẠNG THÁI";
            this.colStatus.Name = "colStatus";
            this.colStatus.Width = 100;

            this.colCreatedAt.HeaderText = "NGÀY TẠO / ĐĂNG";
            this.colCreatedAt.Name = "colCreatedAt";
            this.colCreatedAt.Width = 110;

            this.colAction.HeaderText = "THAO TÁC";
            this.colAction.Name = "colAction";
            this.colAction.Width = 90;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.pnlCard4);
            this.Controls.Add(this.pnlCard3);
            this.Controls.Add(this.pnlCard2);
            this.Controls.Add(this.pnlCard1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmAdminUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cinéluxe Management Studio - Quản lý Người dùng";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCard1.ResumeLayout(false);
            this.pnlCard1.PerformLayout();
            this.pnlCard2.ResumeLayout(false);
            this.pnlCard2.PerformLayout();
            this.pnlCard3.ResumeLayout(false);
            this.pnlCard3.PerformLayout();
            this.pnlCard4.ResumeLayout(false);
            this.pnlCard4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
        }

        public System.Windows.Forms.Panel pnlSidebar;
        public System.Windows.Forms.Label lblLogo;
        public System.Windows.Forms.LinkLabel lnkPos;
        public System.Windows.Forms.LinkLabel lnkCombo;
        public System.Windows.Forms.LinkLabel lnkUsers;
        public System.Windows.Forms.LinkLabel lnkReports;
        public System.Windows.Forms.LinkLabel lnkOccupancy;
        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblSubTitle;
        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.Button btnAddUser;
        public System.Windows.Forms.Panel pnlCard1;
        public System.Windows.Forms.Label lblCardTitle1;
        public System.Windows.Forms.Label lblTotalAccounts;
        public System.Windows.Forms.Panel pnlCard2;
        public System.Windows.Forms.Label lblCardTitle2;
        public System.Windows.Forms.Label lblAdminCount;
        public System.Windows.Forms.Panel pnlCard3;
        public System.Windows.Forms.Label lblCardTitle3;
        public System.Windows.Forms.Label lblStaffCount;
        public System.Windows.Forms.Panel pnlCard4;
        public System.Windows.Forms.Label lblCardTitle4;
        public System.Windows.Forms.Label lblCustomerCount;
        public System.Windows.Forms.DataGridView dgvUsers;
        public System.Windows.Forms.DataGridViewTextBoxColumn colId;
        public System.Windows.Forms.DataGridViewTextBoxColumn colUsername;
        public System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        public System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        public System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        public System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        public System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
        public System.Windows.Forms.DataGridViewTextBoxColumn colAction;
    }
}