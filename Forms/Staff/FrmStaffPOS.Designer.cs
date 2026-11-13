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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lnkReports = new System.Windows.Forms.LinkLabel();
            this.lnkUsers = new System.Windows.Forms.LinkLabel();
            this.lnkCombo = new System.Windows.Forms.LinkLabel();
            this.lnkPos = new System.Windows.Forms.LinkLabel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlComboCatalog = new System.Windows.Forms.Panel();
            this.dgvOrderItems = new System.Windows.Forms.DataGridView();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTotalLabel = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.txtCashGiven = new System.Windows.Forms.TextBox();
            this.lblChangeDue = new System.Windows.Forms.Label();
            this.btnCheckout = new System.Windows.Forms.Button();
            this.btnCancelOrder = new System.Windows.Forms.Button();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).BeginInit();
            this.SuspendLayout();

            this.pnlSidebar.BackColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
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

            this.lnkReports.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))),
                ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkReports.AutoSize = true;
            this.lnkReports.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkReports.LinkColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkReports.Location = new System.Drawing.Point(20, 245);
            this.lnkReports.Name = "lnkReports";
            this.lnkReports.Size = new System.Drawing.Size(155, 19);
            this.lnkReports.TabIndex = 4;
            this.lnkReports.TabStop = true;
            this.lnkReports.Text = "Tài Chính & Doanh Thu";

            this.lnkUsers.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))),
                ((int)(((byte)(248)))));
            this.lnkUsers.AutoSize = true;
            this.lnkUsers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkUsers.LinkColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkUsers.Location = new System.Drawing.Point(20, 190);
            this.lnkUsers.Name = "lnkUsers";
            this.lnkUsers.Size = new System.Drawing.Size(127, 19);
            this.lnkUsers.TabIndex = 3;
            this.lnkUsers.TabStop = true;
            this.lnkUsers.Text = "Quản Lý Nhân Viên";

            this.lnkCombo.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))),
                ((int)(((byte)(248)))));
            this.lnkCombo.AutoSize = true;
            this.lnkCombo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkCombo.LinkColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkCombo.Location = new System.Drawing.Point(20, 135);
            this.lnkCombo.Name = "lnkCombo";
            this.lnkCombo.Size = new System.Drawing.Size(120, 19);
            this.lnkCombo.TabIndex = 2;
            this.lnkCombo.TabStop = true;
            this.lnkCombo.Text = "Quản Lý Combo FB";

            this.lnkPos.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))),
                ((int)(((byte)(248)))));
            this.lnkPos.AutoSize = true;
            this.lnkPos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkPos.LinkColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
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

            this.pnlComboCatalog.BackColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlComboCatalog.Location = new System.Drawing.Point(230, 500);
            this.pnlComboCatalog.Name = "pnlComboCatalog";
            this.pnlComboCatalog.Size = new System.Drawing.Size(515, 370);
            this.pnlComboCatalog.TabIndex = 1;

            this.dgvOrderItems.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))),
                ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.dgvOrderItems.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                this.colItemName,
                this.colQuantity,
                this.colPrice
            });
            this.dgvOrderItems.Location = new System.Drawing.Point(760, 10);
            this.dgvOrderItems.Name = "dgvOrderItems";
            this.dgvOrderItems.RowHeadersVisible = false;
            this.dgvOrderItems.Size = new System.Drawing.Size(500, 480);
            this.dgvOrderItems.TabIndex = 2;

            this.colItemName.HeaderText = "DIỄN GIẢI DỊCH VỤ / MẶT HÀNG";
            this.colItemName.Name = "colItemName";
            this.colItemName.Width = 320;

            this.colQuantity.HeaderText = "SL";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Width = 60;

            this.colPrice.HeaderText = "THÀNH TIỀN";
            this.colPrice.Name = "colPrice";
            this.colPrice.Width = 118;

            this.lblTotalLabel.AutoSize = true;
            this.lblTotalLabel.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalLabel.ForeColor = System.Drawing.Color.White;
            this.lblTotalLabel.Location = new System.Drawing.Point(760, 510);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new System.Drawing.Size(71, 37);
            this.lblTotalLabel.TabIndex = 3;
            this.lblTotalLabel.Text = "0 đ";

            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.White;
            this.lblTotalAmount.Location = new System.Drawing.Point(760, 510);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(71, 37);
            this.lblTotalAmount.TabIndex = 3;
            this.lblTotalAmount.Text = "0 đ";

            this.txtCashGiven.BackColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.txtCashGiven.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCashGiven.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtCashGiven.ForeColor = System.Drawing.Color.White;
            this.txtCashGiven.Location = new System.Drawing.Point(760, 570);
            this.txtCashGiven.Name = "txtCashGiven";
            this.txtCashGiven.Size = new System.Drawing.Size(500, 29);
            this.txtCashGiven.TabIndex = 4;
            this.txtCashGiven.Text = "500000";

            this.lblChangeDue.AutoSize = true;
            this.lblChangeDue.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblChangeDue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))),
                ((int)(((byte)(248)))));
            this.lblChangeDue.Location = new System.Drawing.Point(760, 615);
            this.lblChangeDue.Name = "lblChangeDue";
            this.lblChangeDue.Size = new System.Drawing.Size(250, 17);
            this.lblChangeDue.TabIndex = 5;
            this.lblChangeDue.Text = "TIỀN THỐI LẠI (CHANGE DUE): 500.000 đ";

            this.btnCheckout.BackColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.btnCheckout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCheckout.ForeColor = System.Drawing.Color.White;
            this.btnCheckout.Location = new System.Drawing.Point(760, 650);
            this.btnCheckout.Name = "btnCheckout";
            this.btnCheckout.Size = new System.Drawing.Size(500, 50);
            this.btnCheckout.TabIndex = 6;
            this.btnCheckout.Text = "IN VÉ HOÀN TẤT GIAO DỊCH [Enter]";
            this.btnCheckout.UseVisualStyleBackColor = false;

            this.btnCancelOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(29)))),
                ((int)(((byte)(72)))));
        this.btnCancelOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelOrder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelOrder.ForeColor = System.Drawing.Color.White;
            this.btnCancelOrder.Location = new System.Drawing.Point(760, 715);
            this.btnCancelOrder.Name = "btnCancelOrder";
            this.btnCancelOrder.Size = new System.Drawing.Size(500, 40);
            this.btnCancelOrder.TabIndex = 7;
            this.btnCancelOrder.Text = "Hủy giao dịch [Esc]";
            this.btnCancelOrder.UseVisualStyleBackColor = false;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.Controls.Add(this.btnCancelOrder);
            this.Controls.Add(this.btnCheckout);
            this.Controls.Add(this.lblChangeDue);
            this.Controls.Add(this.txtCashGiven);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblTotalLabel);
            this.Controls.Add(this.dgvOrderItems);
            this.Controls.Add(this.pnlComboCatalog);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmStaffPOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CINELUXE MANAGEMENT STUDIO - BÁN VÉ QUẦY POS";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.LinkLabel lnkPos;
        private System.Windows.Forms.LinkLabel lnkCombo;
        private System.Windows.Forms.LinkLabel lnkUsers;
        private System.Windows.Forms.LinkLabel lnkReports;
        private System.Windows.Forms.Panel pnlComboCatalog;
        private System.Windows.Forms.DataGridView dgvOrderItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.Label lblTotalLabel;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.TextBox txtCashGiven;
        private System.Windows.Forms.Label lblChangeDue;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.Button btnCancelOrder;
    }
}