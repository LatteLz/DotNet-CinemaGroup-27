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
            this.lnkUsers = new System.Windows.Forms.LinkLabel();
            this.lnkCombo = new System.Windows.Forms.LinkLabel();
            this.lnkPos = new System.Windows.Forms.LinkLabel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblSubHeader = new System.Windows.Forms.Label();
            this.pnlStats1 = new System.Windows.Forms.Panel();
            this.lblTotalAccountsTitle = new System.Windows.Forms.Label();
            this.lblTotalAccounts = new System.Windows.Forms.Label();
            this.pnlStats2 = new System.Windows.Forms.Panel();
            this.lblAdminCountTitle = new System.Windows.Forms.Label();
            this.lblAdminCount = new System.Windows.Forms.Label();
            this.pnlStats3 = new System.Windows.Forms.Panel();
            this.lblStaffCountTitle = new System.Windows.Forms.Label();
            this.lblStaffCount = new System.Windows.Forms.Label();
            this.pnlStats4 = new System.Windows.Forms.Panel();
            this.lblCustomerCountTitle = new System.Windows.Forms.Label();
            this.lblCustomerCount = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnAddUser = new System.Windows.Forms.Button();
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
            this.pnlStats1.SuspendLayout();
            this.pnlStats2.SuspendLayout();
            this.pnlStats3.SuspendLayout();
            this.pnlStats4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();

            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlSidebar.Controls.Add(this.lnkUsers);
            this.pnlSidebar.Controls.Add(this.lnkCombo);
            this.pnlSidebar.Controls.Add(this.lnkPos);
            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(220, 900);
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

            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(240, 20);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(193, 30);
            this.lblHeaderTitle.TabIndex = 1;
            this.lblHeaderTitle.Text = "QUẢN LÝ NGƯỜI DÙNG";

            this.lblSubHeader.AutoSize = true;
            this.lblSubHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lblSubHeader.Location = new System.Drawing.Point(240, 55);
            this.lblSubHeader.Name = "lblSubHeader";
            this.lblSubHeader.Size = new System.Drawing.Size(235, 15);
            this.lblSubHeader.TabIndex = 2;
            this.lblSubHeader.Text = "BẢO MẬT PHÂN QUYỀN RBAC-LEVEL 4 [ACTIVE]";

            this.pnlStats1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlStats1.Controls.Add(this.lblTotalAccounts);
            this.pnlStats1.Controls.Add(this.lblTotalAccountsTitle);
            this.pnlStats1.Location = new System.Drawing.Point(240, 90);
            this.pnlStats1.Name = "pnlStats1";
            this.pnlStats1.Size = new System.Drawing.Size(270, 75);
            this.pnlStats1.TabIndex = 3;

            this.lblTotalAccountsTitle.AutoSize = true;
            this.lblTotalAccountsTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblTotalAccountsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblTotalAccountsTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTotalAccountsTitle.Name = "lblTotalAccountsTitle";
            this.lblTotalAccountsTitle.Size = new System.Drawing.Size(89, 15);
            this.lblTotalAccountsTitle.TabIndex = 0;
            this.lblTotalAccountsTitle.Text = "TỔNG TÀI KHOẢN";

            this.lblTotalAccounts.AutoSize = true;
            this.lblTotalAccounts.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotalAccounts.ForeColor = System.Drawing.Color.White;
            this.lblTotalAccounts.Location = new System.Drawing.Point(10, 30);
            this.lblTotalAccounts.Name = "lblTotalAccounts";
            this.lblTotalAccounts.Size = new System.Drawing.Size(64, 30);
            this.lblTotalAccounts.TabIndex = 1;
            this.lblTotalAccounts.Text = "1,428";

            this.pnlStats2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlStats2.Controls.Add(this.lblAdminCount);
            this.pnlStats2.Controls.Add(this.lblAdminCountTitle);
            this.pnlStats2.Location = new System.Drawing.Point(525, 90);
            this.pnlStats2.Name = "pnlStats2";
            this.pnlStats2.Size = new System.Drawing.Size(270, 75);
            this.pnlStats2.TabIndex = 4;

            this.lblAdminCountTitle.AutoSize = true;
            this.lblAdminCountTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblAdminCountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblAdminCountTitle.Location = new System.Drawing.Point(12, 10);
            this.lblAdminCountTitle.Name = "lblAdminCountTitle";
            this.lblAdminCountTitle.Size = new System.Drawing.Size(126, 15);
            this.lblAdminCountTitle.TabIndex = 0;
            this.lblAdminCountTitle.Text = "QUẢN TRỊ VIÊN (ADMIN)";

            this.lblAdminCount.AutoSize = true;
            this.lblAdminCount.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAdminCount.ForeColor = System.Drawing.Color.White;
            this.lblAdminCount.Location = new System.Drawing.Point(10, 30);
            this.lblAdminCount.Name = "lblAdminCount";
            this.lblAdminCount.Size = new System.Drawing.Size(25, 30);
            this.lblAdminCount.TabIndex = 1;
            this.lblAdminCount.Text = "4";

            this.pnlStats3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlStats3.Controls.Add(this.lblStaffCount);
            this.pnlStats3.Controls.Add(this.lblStaffCountTitle);
            this.pnlStats3.Location = new System.Drawing.Point(810, 90);
            this.pnlStats3.Name = "pnlStats3";
            this.pnlStats3.Size = new System.Drawing.Size(270, 75);
            this.pnlStats3.TabIndex = 5;

            this.lblStaffCountTitle.AutoSize = true;
            this.lblStaffCountTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStaffCountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblStaffCountTitle.Location = new System.Drawing.Point(12, 10);
            this.lblStaffCountTitle.Name = "lblStaffCountTitle";
            this.lblStaffCountTitle.Size = new System.Drawing.Size(142, 15);
            this.lblStaffCountTitle.TabIndex = 0;
            this.lblStaffCountTitle.Text = "NHÂN VIÊN TRỰC CA (STAFF)";

            this.lblStaffCount.AutoSize = true;
            this.lblStaffCount.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblStaffCount.ForeColor = System.Drawing.Color.White;
            this.lblStaffCount.Location = new System.Drawing.Point(10, 30);
            this.lblStaffCount.Name = "lblStaffCount";
            this.lblStaffCount.Size = new System.Drawing.Size(37, 30);
            this.lblStaffCount.TabIndex = 1;
            this.lblStaffCount.Text = "24";

            this.pnlStats4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlStats4.Controls.Add(this.lblCustomerCount);
            this.pnlStats4.Controls.Add(this.lblCustomerCountTitle);
            this.pnlStats4.Location = new System.Drawing.Point(1095, 90);
            this.pnlStats4.Name = "pnlStats4";
            this.pnlStats4.Size = new System.Drawing.Size(165, 75);
            this.pnlStats4.TabIndex = 6;

            this.lblCustomerCountTitle.AutoSize = true;
            this.lblCustomerCountTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCustomerCountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCustomerCountTitle.Location = new System.Drawing.Point(12, 10);
            this.lblCustomerCountTitle.Name = "lblCustomerCountTitle";
            this.lblCustomerCountTitle.Size = new System.Drawing.Size(117, 15);
            this.lblCustomerCountTitle.TabIndex = 0;
            this.lblCustomerCountTitle.Text = "HỘI VIÊN (CUSTOMERS)";

            this.lblCustomerCount.AutoSize = true;
            this.lblCustomerCount.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCustomerCount.ForeColor = System.Drawing.Color.White;
            this.lblCustomerCount.Location = new System.Drawing.Point(10, 30);
            this.lblCustomerCount.Name = "lblCustomerCount";
            this.lblCustomerCount.Size = new System.Drawing.Size(64, 30);
            this.lblCustomerCount.TabIndex = 1;
            this.lblCustomerCount.Text = "1,400";

            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(240, 185);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(250, 25);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.Text = "Tìm kiếm theo tên, email, username...";

            this.btnAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAddUser.ForeColor = System.Drawing.Color.Black;
            this.btnAddUser.Location = new System.Drawing.Point(1110, 182);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(150, 32);
            this.btnAddUser.TabIndex = 8;
            this.btnAddUser.Text = "+ Thêm Người Dùng";
            this.btnAddUser.UseVisualStyleBackColor = false;

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
            this.dgvUsers.Location = new System.Drawing.Point(240, 230);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.Size = new System.Drawing.Size(1020, 630);
            this.dgvUsers.TabIndex = 9;

            this.colId.HeaderText = "MÃ ID";
            this.colId.Name = "colId";
            this.colId.Width = 100;

            this.colUsername.HeaderText = "NGƯỜI DÙNG / TÀI KHOẢN";
            this.colUsername.Name = "colUsername";
            this.colUsername.Width = 200;

            this.colEmail.HeaderText = "LIÊN HỆ EMAIL";
            this.colEmail.Name = "colEmail";
            this.colEmail.Width = 200;

            this.colPhone.HeaderText = "SỐ ĐIỆN THOẠI";
            this.colPhone.Name = "colPhone";
            this.colPhone.Width = 120;

            this.colRole.HeaderText = "VAI TRÒ / NHIỆM VỤ";
            this.colRole.Name = "colRole";
            this.colRole.Width = 150;

            this.colStatus.HeaderText = "TRẠNG THÁI";
            this.colStatus.Name = "colStatus";
            this.colStatus.Width = 100;

            this.colCreatedAt.HeaderText = "NGÀY TẠO / ĐĂNG";
            this.colCreatedAt.Name = "colCreatedAt";
            this.colCreatedAt.Width = 110;

            this.colAction.HeaderText = "THAO TÁC";
            this.colAction.Name = "colAction";
            this.colAction.Width = 70;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.pnlStats4);
            this.Controls.Add(this.pnlStats3);
            this.Controls.Add(this.pnlStats2);
            this.Controls.Add(this.pnlStats1);
            this.Controls.Add(this.lblSubHeader);
            this.Controls.Add(this.lblHeaderTitle);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmAdminUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CINELUXE MANAGEMENT STUDIO - QUẢN LÝ NGƯỜI DÙNG";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlStats1.ResumeLayout(false);
            this.pnlStats1.PerformLayout();
            this.pnlStats2.ResumeLayout(false);
            this.pnlStats2.PerformLayout();
            this.pnlStats3.ResumeLayout(false);
            this.pnlStats3.PerformLayout();
            this.pnlStats4.ResumeLayout(false);
            this.pnlStats4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.LinkLabel lnkPos;
        private System.Windows.Forms.LinkLabel lnkCombo;
        private System.Windows.Forms.LinkLabel lnkUsers;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblSubHeader;
        private System.Windows.Forms.Panel pnlStats1;
        private System.Windows.Forms.Label lblTotalAccountsTitle;
        private System.Windows.Forms.Label lblTotalAccounts;
        private System.Windows.Forms.Panel pnlStats2;
        private System.Windows.Forms.Label lblAdminCountTitle;
        private System.Windows.Forms.Label lblAdminCount;
        private System.Windows.Forms.Panel pnlStats3;
        private System.Windows.Forms.Label lblStaffCountTitle;
        private System.Windows.Forms.Label lblStaffCount;
        private System.Windows.Forms.Panel pnlStats4;
        private System.Windows.Forms.Label lblCustomerCountTitle;
        private System.Windows.Forms.Label lblCustomerCount;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsername;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
    }
}