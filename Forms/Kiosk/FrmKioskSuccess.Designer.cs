using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Kiosk;

partial class FrmKioskSuccess
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblLogo;
        private Label lblClock;
        private Panel pnlTicketCard;
        private Label lblSuccessIcon;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlQrContainer;
        private PictureBox picTicketQr;
        private Label lblTicketCode;
        private Label lblTicketDetails;
        private Button btnFinish;
        private Label lblAutoRedirect;

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
            this.Text = "CineLuxe Kiosk - Đặt vé thành công";
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

            lblClock = new Label
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(1780, 26),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblClock);

            pnlTicketCard = new Panel
            {
                Size = new Size(860, 860),
                Location = new Point(530, 130),
                BackColor = ColorTranslator.FromHtml("#181c20")
            };

            lblSuccessIcon = new Label
            {
                Text = "✔",
                Font = new Font("Segoe UI", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#10B981"),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(84, 84),
                Location = new Point(388, 32)
            };

            lblTitle = new Label
            {
                Text = "ĐẶT VÉ THÀNH CÔNG!",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#10B981"),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(800, 45),
                Location = new Point(30, 130)
            };

            lblSubtitle = new Label
            {
                Text = "Vui lòng nhận vé hoặc chụp lại mã QR bên dưới để quét tại cổng soát vé",
                Font = new Font("Segoe UI", 11.5F),
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(800, 30),
                Location = new Point(30, 180)
            };

            pnlQrContainer = new Panel
            {
                Size = new Size(250, 250),
                Location = new Point(305, 225),
                BackColor = Color.White,
                Padding = new Padding(12)
            };

            picTicketQr = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.White
            };
            pnlQrContainer.Controls.Add(picTicketQr);

            lblTicketCode = new Label
            {
                Text = "MÃ VÉ: #CLX-000000",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(800, 36),
                Location = new Point(30, 490)
            };

            lblTicketDetails = new Label
            {
                Text = "Đang tải thông tin vé...",
                Font = new Font("Segoe UI", 11.5F),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(760, 175),
                Location = new Point(50, 535)
            };

            btnFinish = new Button
            {
                Text = "HOÀN TẤT & VỀ TRANG CHỦ",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#38bdf8"),
                ForeColor = ColorTranslator.FromHtml("#101418"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(460, 62),
                Location = new Point(200, 725),
                Cursor = Cursors.Hand
            };
            btnFinish.FlatAppearance.BorderSize = 0;

            lblAutoRedirect = new Label
            {
                Text = "Tự động quay về màn hình chính sau 15 giây...",
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(800, 28),
                Location = new Point(30, 800)
            };

            pnlTicketCard.Controls.AddRange(new Control[]
            {
                lblSuccessIcon, lblTitle, lblSubtitle, pnlQrContainer,
                lblTicketCode, lblTicketDetails, btnFinish, lblAutoRedirect
            });

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlTicketCard);
            this.ResumeLayout(false);
        }

        #endregion
    }