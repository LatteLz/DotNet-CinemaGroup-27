using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Kiosk;

    partial class FrmKioskPayment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblLogo;
        private Label lblStep1;
        private Label lblStep2;
        private Label lblStep3;
        private Label lblStep4;
        private Label lblClock;
        private Button btnBack;
        private Button btnCancel;
        private Panel pnlLeft;
        private Label lblSummaryTitle;
        private Panel pnlOrderDetails;
        private Panel pnlRight;
        private Label lblPayTitle;
        private Button btnQr;
        private Button btnCard;
        private Button btnCash;
        private Label lblSelectedMethod;
        private Panel pnlQrBox;
        private Panel qrPlaceholder;
        private Label lblQrMock;
        private Label lblQrInstruction;
        private Button btnConfirmPay;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "CineLuxe Kiosk - Thanh toán";
            this.Size = new Size(1920, 1080);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ColorTranslator.FromHtml("#101418");

            pnlHeader = new Panel
            {
                Size = new Size(1920, 80),
                Location = new Point(0, 0),
                BackColor = ColorTranslator.FromHtml("#181c20")
            };

            lblLogo = new Label
            {
                Text = "CINELUXE KIOSK",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(24, 24),
                AutoSize = true
            };

            lblStep1 = new Label
            {
                Text = "1. Chọn Phim",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                Location = new Point(440, 28),
                AutoSize = true
            };

            lblStep2 = new Label
            {
                Text = "2. Chọn Suất",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                Location = new Point(600, 28),
                AutoSize = true
            };

            lblStep3 = new Label
            {
                Text = "3. Chọn Ghế & Combo",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                Location = new Point(760, 28),
                AutoSize = true
            };

            lblStep4 = new Label
            {
                Text = "4. Thanh Toán",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#1a5276"),
                Padding = new Padding(14, 8, 14, 8),
                Location = new Point(980, 20),
                AutoSize = true
            };

            lblClock = new Label
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(1415, 26),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnBack = new Button
            {
                Text = "← Quay lại",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#262a2f"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 36),
                Location = new Point(1530, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;

            btnCancel = new Button
            {
                Text = "✕ HỦY GIAO DỊCH",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#ef4444"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(185, 36),
                Location = new Point(1680, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            pnlHeader.Controls.AddRange(new Control[] { lblLogo, lblStep1, lblStep2, lblStep3, lblStep4, lblClock, btnBack, btnCancel });

            pnlLeft = new Panel
            {
                Size = new Size(760, 920),
                Location = new Point(40, 110),
                BackColor = ColorTranslator.FromHtml("#181c20"),
                Padding = new Padding(30)
            };

            lblSummaryTitle = new Label
            {
                Text = "CHI TIẾT ĐƠN ĐẶT VÉ",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(30, 25),
                AutoSize = true
            };

            pnlOrderDetails = new Panel
            {
                Size = new Size(700, 820),
                Location = new Point(30, 75),
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#181c20")
            };

            pnlLeft.Controls.Add(lblSummaryTitle);
            pnlLeft.Controls.Add(pnlOrderDetails);

            pnlRight = new Panel
            {
                Size = new Size(1040, 920),
                Location = new Point(840, 110),
                BackColor = ColorTranslator.FromHtml("#181c20")
            };

            lblPayTitle = new Label
            {
                Text = "CHỌN PHƯƠNG THỨC THANH TOÁN",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(40, 25),
                AutoSize = true
            };

            btnQr = new Button
            {
                Text = "Quét mã QR (MoMo / Ngân hàng)",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#1a5276"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(300, 70),
                Location = new Point(40, 80),
                Cursor = Cursors.Hand
            };
            btnQr.FlatAppearance.BorderSize = 0;

            btnCard = new Button
            {
                Text = "Thẻ ATM / Visa / Mastercard (POS)",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                BackColor = ColorTranslator.FromHtml("#262a2f"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(300, 70),
                Location = new Point(370, 80),
                Cursor = Cursors.Hand
            };
            btnCard.FlatAppearance.BorderSize = 0;

            btnCash = new Button
            {
                Text = "Thanh toán tại quầy thu ngân",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                BackColor = ColorTranslator.FromHtml("#262a2f"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(300, 70),
                Location = new Point(700, 80),
                Cursor = Cursors.Hand
            };
            btnCash.FlatAppearance.BorderSize = 0;

            lblSelectedMethod = new Label
            {
                Text = "Phương thức đang chọn: Quét mã QR Ngân hàng / Ví điện tử",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#38bdf8"),
                Location = new Point(40, 175),
                AutoSize = true
            };

            pnlQrBox = new Panel
            {
                Size = new Size(960, 520),
                Location = new Point(40, 220),
                BackColor = ColorTranslator.FromHtml("#1c2024")
            };

            qrPlaceholder = new Panel
            {
                Size = new Size(260, 260),
                Location = new Point(350, 60),
                BackColor = Color.White
            };

            lblQrMock = new Label
            {
                Text = "[ MÃ QR THANH TOÁN ]",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            qrPlaceholder.Controls.Add(lblQrMock);

            lblQrInstruction = new Label
            {
                Text = "Mở ứng dụng Ngân hàng hoặc Ví điện tử để quét mã QR.\nHệ thống sẽ tự động xuất vé ngay sau khi giao dịch thành công.",
                Font = new Font("Segoe UI", 12F),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(900, 80),
                Location = new Point(30, 350)
            };

            pnlQrBox.Controls.Add(qrPlaceholder);
            pnlQrBox.Controls.Add(lblQrInstruction);

            btnConfirmPay = new Button
            {
                Text = "XÁC NHẬN ĐÃ THANH TOÁN",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#38bdf8"),
                ForeColor = ColorTranslator.FromHtml("#101418"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(960, 75),
                Location = new Point(40, 790),
                Cursor = Cursors.Hand
            };
            btnConfirmPay.FlatAppearance.BorderSize = 0;

            pnlRight.Controls.AddRange(new Control[] { lblPayTitle, btnQr, btnCard, btnCash, lblSelectedMethod, pnlQrBox, btnConfirmPay });

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlLeft);
            this.Controls.Add(pnlRight);
            this.ResumeLayout(false);
        }

        #endregion
    }