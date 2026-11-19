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
            this.lnkOccupancy = new System.Windows.Forms.LinkLabel();
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
            this.pnlRightQrSlot = new System.Windows.Forms.Panel();
            this.lblRightQrTitle = new System.Windows.Forms.Label();
            this.picRightQr = new System.Windows.Forms.PictureBox();
            this.lblRightQrCaption = new System.Windows.Forms.Label();
            this.lblTotalLabel = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.pnlQrBox = new System.Windows.Forms.Panel();
            this.rdoCashMethod = new System.Windows.Forms.RadioButton();
            this.rdoQrMethod = new System.Windows.Forms.RadioButton();
            this.lblQrHint = new System.Windows.Forms.Label();
            this.picQrCode = new System.Windows.Forms.PictureBox();
            this.pnlCashBox = new System.Windows.Forms.Panel();
            this.lblCashInputTitle = new System.Windows.Forms.Label();
            this.txtCashGiven = new System.Windows.Forms.TextBox();
            this.lblChangeDue = new System.Windows.Forms.Label();
            this.btnCheckout = new System.Windows.Forms.Button();
            this.btnCancelOrder = new System.Windows.Forms.Button();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).BeginInit();
            this.pnlRightQrSlot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRightQr)).BeginInit();
            this.pnlQrBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picQrCode)).BeginInit();
            this.pnlCashBox.SuspendLayout();
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
            this.pnlSidebar.Size = new System.Drawing.Size(220, 900);
            this.pnlSidebar.TabIndex = 0;

            this.lnkOccupancy.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkOccupancy.AutoSize = true;
            this.lnkOccupancy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkOccupancy.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkOccupancy.Location = new System.Drawing.Point(20, 300);
            this.lnkOccupancy.Name = "lnkOccupancy";
            this.lnkOccupancy.Size = new System.Drawing.Size(156, 19);
            this.lnkOccupancy.TabIndex = 5;
            this.lnkOccupancy.TabStop = true;
            this.lnkOccupancy.Text = "Lấp đầy Khán Phòng";

            this.lnkReports.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkReports.AutoSize = true;
            this.lnkReports.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lnkReports.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lnkReports.Location = new System.Drawing.Point(20, 245);
            this.lnkReports.Name = "lnkReports";
            this.lnkReports.Size = new System.Drawing.Size(155, 19);
            this.lnkReports.TabIndex = 4;
            this.lnkReports.TabStop = true;
            this.lnkReports.Text = "Tài Chính & Doanh Thu";

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

            this.pnlComboCatalog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlComboCatalog.Location = new System.Drawing.Point(230, 500);
            this.pnlComboCatalog.Name = "pnlComboCatalog";
            this.pnlComboCatalog.Size = new System.Drawing.Size(515, 370);
            this.pnlComboCatalog.TabIndex = 1;

            this.dgvOrderItems.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.dgvOrderItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemName,
            this.colQuantity,
            this.colPrice});
            this.dgvOrderItems.Location = new System.Drawing.Point(760, 10);
            this.dgvOrderItems.Name = "dgvOrderItems";
            this.dgvOrderItems.RowHeadersVisible = false;
            this.dgvOrderItems.Size = new System.Drawing.Size(500, 165);
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

            this.pnlRightQrSlot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.pnlRightQrSlot.Controls.Add(this.lblRightQrTitle);
            this.pnlRightQrSlot.Controls.Add(this.picRightQr);
            this.pnlRightQrSlot.Controls.Add(this.lblRightQrCaption);
            this.pnlRightQrSlot.Location = new System.Drawing.Point(760, 182);
            this.pnlRightQrSlot.Name = "pnlRightQrSlot";
            this.pnlRightQrSlot.Size = new System.Drawing.Size(500, 306);
            this.pnlRightQrSlot.TabIndex = 8;
            this.pnlRightQrSlot.Visible = false;

            this.lblRightQrTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRightQrTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lblRightQrTitle.Location = new System.Drawing.Point(0, 8);
            this.lblRightQrTitle.Name = "lblRightQrTitle";
            this.lblRightQrTitle.Size = new System.Drawing.Size(500, 24);
            this.lblRightQrTitle.Text = "QUÉT MÃ QR NGÂN HÀNG / VÍ ĐIỆN TỬ";
            this.lblRightQrTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.picRightQr.BackColor = System.Drawing.Color.White;
            this.picRightQr.Location = new System.Drawing.Point(145, 38);
            this.picRightQr.Name = "picRightQr";
            this.picRightQr.Padding = new System.Windows.Forms.Padding(4);
            this.picRightQr.Size = new System.Drawing.Size(210, 210);
            this.picRightQr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRightQr.TabIndex = 0;
            this.picRightQr.TabStop = false;

            this.lblRightQrCaption.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRightQrCaption.ForeColor = System.Drawing.Color.White;
            this.lblRightQrCaption.Location = new System.Drawing.Point(0, 256);
            this.lblRightQrCaption.Name = "lblRightQrCaption";
            this.lblRightQrCaption.Size = new System.Drawing.Size(500, 28);
            this.lblRightQrCaption.Text = "Số tiền thanh toán: 0 VNĐ";
            this.lblRightQrCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblTotalLabel.AutoSize = true;
            this.lblTotalLabel.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalLabel.ForeColor = System.Drawing.Color.White;
            this.lblTotalLabel.Location = new System.Drawing.Point(760, 496);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new System.Drawing.Size(71, 37);
            this.lblTotalLabel.TabIndex = 3;
            this.lblTotalLabel.Text = "0 đ";

            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.White;
            this.lblTotalAmount.Location = new System.Drawing.Point(760, 496);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(71, 37);
            this.lblTotalAmount.TabIndex = 3;
            this.lblTotalAmount.Text = "0 đ";

            this.pnlQrBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.pnlQrBox.Controls.Add(this.rdoCashMethod);
            this.pnlQrBox.Controls.Add(this.rdoQrMethod);
            this.pnlQrBox.Controls.Add(this.lblQrHint);
            this.pnlQrBox.Controls.Add(this.picQrCode);
            this.pnlQrBox.Location = new System.Drawing.Point(760, 540);
            this.pnlQrBox.Name = "pnlQrBox";
            this.pnlQrBox.Size = new System.Drawing.Size(500, 58);
            this.pnlQrBox.TabIndex = 9;

            this.rdoCashMethod.AutoSize = true;
            this.rdoCashMethod.Checked = true;
            this.rdoCashMethod.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.rdoCashMethod.ForeColor = System.Drawing.Color.White;
            this.rdoCashMethod.Location = new System.Drawing.Point(8, 6);
            this.rdoCashMethod.Name = "rdoCashMethod";
            this.rdoCashMethod.Size = new System.Drawing.Size(74, 19);
            this.rdoCashMethod.TabIndex = 0;
            this.rdoCashMethod.TabStop = true;
            this.rdoCashMethod.Text = "Tiền mặt";

            this.rdoQrMethod.AutoSize = true;
            this.rdoQrMethod.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.rdoQrMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.rdoQrMethod.Location = new System.Drawing.Point(92, 6);
            this.rdoQrMethod.Name = "rdoQrMethod";
            this.rdoQrMethod.Size = new System.Drawing.Size(64, 19);
            this.rdoQrMethod.TabIndex = 1;
            this.rdoQrMethod.Text = "Mã QR";

            this.lblQrHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblQrHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblQrHint.Location = new System.Drawing.Point(8, 30);
            this.lblQrHint.Name = "lblQrHint";
            this.lblQrHint.Size = new System.Drawing.Size(436, 22);
            this.lblQrHint.Text = "Thanh toán tiền mặt tại quầy";

            this.picQrCode.BackColor = System.Drawing.Color.White;
            this.picQrCode.Location = new System.Drawing.Point(450, 6);
            this.picQrCode.Name = "picQrCode";
            this.picQrCode.Size = new System.Drawing.Size(42, 42);
            this.picQrCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picQrCode.TabIndex = 2;
            this.picQrCode.TabStop = false;
            this.picQrCode.Visible = false;

            this.pnlCashBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.pnlCashBox.Controls.Add(this.lblCashInputTitle);
            this.pnlCashBox.Controls.Add(this.txtCashGiven);
            this.pnlCashBox.Controls.Add(this.lblChangeDue);
            this.pnlCashBox.Location = new System.Drawing.Point(760, 604);
            this.pnlCashBox.Name = "pnlCashBox";
            this.pnlCashBox.Size = new System.Drawing.Size(500, 82);
            this.pnlCashBox.TabIndex = 10;

            this.lblCashInputTitle.AutoSize = true;
            this.lblCashInputTitle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCashInputTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCashInputTitle.Location = new System.Drawing.Point(8, 4);
            this.lblCashInputTitle.Name = "lblCashInputTitle";
            this.lblCashInputTitle.Size = new System.Drawing.Size(119, 13);
            this.lblCashInputTitle.Text = "SỐ TIỀN KHÁCH ĐƯA:";

            this.txtCashGiven.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.txtCashGiven.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCashGiven.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtCashGiven.ForeColor = System.Drawing.Color.White;
            this.txtCashGiven.Location = new System.Drawing.Point(8, 20);
            this.txtCashGiven.Name = "txtCashGiven";
            this.txtCashGiven.Size = new System.Drawing.Size(484, 27);
            this.txtCashGiven.TabIndex = 4;
            this.txtCashGiven.Text = "500000";

            this.lblChangeDue.AutoSize = true;
            this.lblChangeDue.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblChangeDue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lblChangeDue.Location = new System.Drawing.Point(8, 54);
            this.lblChangeDue.Name = "lblChangeDue";
            this.lblChangeDue.Size = new System.Drawing.Size(250, 17);
            this.lblChangeDue.TabIndex = 5;
            this.lblChangeDue.Text = "TIỀN THỐI LẠI (CHANGE DUE): 500.000 đ";

            this.btnCheckout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.btnCheckout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCheckout.ForeColor = System.Drawing.Color.White;
            this.btnCheckout.Location = new System.Drawing.Point(760, 694);
            this.btnCheckout.Name = "btnCheckout";
            this.btnCheckout.Size = new System.Drawing.Size(500, 50);
            this.btnCheckout.TabIndex = 6;
            this.btnCheckout.Text = "IN VÉ HOÀN TẤT GIAO DỊCH [Enter]";
            this.btnCheckout.UseVisualStyleBackColor = false;

            this.btnCancelOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(29)))), ((int)(((byte)(72)))));
            this.btnCancelOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelOrder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelOrder.ForeColor = System.Drawing.Color.White;
            this.btnCancelOrder.Location = new System.Drawing.Point(760, 754);
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
            this.Controls.Add(this.pnlCashBox);
            this.Controls.Add(this.pnlQrBox);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblTotalLabel);
            this.Controls.Add(this.pnlRightQrSlot);
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
            this.pnlRightQrSlot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRightQr)).EndInit();
            this.pnlQrBox.ResumeLayout(false);
            this.pnlQrBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picQrCode)).EndInit();
            this.pnlCashBox.ResumeLayout(false);
            this.pnlCashBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.LinkLabel lnkPos;
        private System.Windows.Forms.LinkLabel lnkCombo;
        private System.Windows.Forms.LinkLabel lnkUsers;
        private System.Windows.Forms.LinkLabel lnkReports;
        private System.Windows.Forms.LinkLabel lnkOccupancy;
        private System.Windows.Forms.Panel pnlComboCatalog;
        private System.Windows.Forms.DataGridView dgvOrderItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.Panel pnlRightQrSlot;
        private System.Windows.Forms.Label lblRightQrTitle;
        private System.Windows.Forms.PictureBox picRightQr;
        private System.Windows.Forms.Label lblRightQrCaption;
        private System.Windows.Forms.Label lblTotalLabel;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Panel pnlQrBox;
        private System.Windows.Forms.RadioButton rdoCashMethod;
        private System.Windows.Forms.RadioButton rdoQrMethod;
        private System.Windows.Forms.Label lblQrHint;
        private System.Windows.Forms.PictureBox picQrCode;
        private System.Windows.Forms.Panel pnlCashBox;
        private System.Windows.Forms.Label lblCashInputTitle;
        private System.Windows.Forms.TextBox txtCashGiven;
        private System.Windows.Forms.Label lblChangeDue;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.Button btnCancelOrder;
    }
}