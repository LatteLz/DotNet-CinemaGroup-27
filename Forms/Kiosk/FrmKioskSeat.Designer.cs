using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Kiosk;
partial class FrmKioskSeat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlLeftInfo;
        private Panel pnlScreenArea;
        private Panel pnlBottomBar;
        private Label lblClock;
        private Label lblMovieTitle;
        private Label lblShowtimeInfo;
        private Label lblSelectedSeats;
        private Label lblTotalPrice;
        private Button btnContinuePayment;
        private Label lblCountdown;
        private Label lblLogo;
        private Label lblStep1;
        private Label lblStep2;
        private Label lblStep3;
        private Button btnBack;
        private Button btnExit;
        private Panel pnlLegend;
        private Label lblLegendTitle;
        private Panel pnlCountdown;
        private Label lblCountdownTitle;
        private Label lblScreen;

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
            this.Text = "CineLuxe Kiosk - Chọn ghế";
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
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(24, 26),
                AutoSize = true
            };

            lblStep1 = new Label
            {
                Text = "1. Chọn Phim",
                Font = new Font("Segoe UI", 11F),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                Location = new Point(550, 28),
                AutoSize = true
            };

            lblStep2 = new Label
            {
                Text = "2. Chọn Suất Chiếu",
                Font = new Font("Segoe UI", 11F),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                Location = new Point(700, 28),
                AutoSize = true
            };

            lblStep3 = new Label
            {
                Text = "3. Chọn Ghế",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#1a5276"),
                Padding = new Padding(16, 8, 16, 8),
                Location = new Point(870, 20),
                AutoSize = true
            };

            lblClock = new Label
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(1530, 26),
                AutoSize = true
            };

            btnBack = new Button
            {
                Text = "← Quay lại",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                BackColor = ColorTranslator.FromHtml("#262a2f"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 38),
                Location = new Point(1660, 21),
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;

            btnExit = new Button
            {
                Text = "✕ THOÁT",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#ef4444"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 38),
                Location = new Point(1780, 21),
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;

            pnlHeader.Controls.AddRange(new Control[] { lblLogo, lblStep1, lblStep2, lblStep3, lblClock, btnBack, btnExit });

            pnlLeftInfo = new Panel
            {
                Size = new Size(380, 876),
                Location = new Point(24, 96),
                BackColor = ColorTranslator.FromHtml("#1c2024")
            };

            lblMovieTitle = new Label
            {
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(20, 20),
                AutoSize = true,
                MaximumSize = new Size(340, 0)
            };

            lblShowtimeInfo = new Label
            {
                Font = new Font("Segoe UI", 10F),
                ForeColor = ColorTranslator.FromHtml("#c1c7cf"),
                Location = new Point(20, 95),
                AutoSize = true
            };

            pnlLegend = new Panel
            {
                Location = new Point(20, 210),
                Size = new Size(340, 180),
                BackColor = ColorTranslator.FromHtml("#181c20")
            };

            lblLegendTitle = new Label
            {
                Text = "CHÚ THÍCH GHẾ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#38bdf8"),
                Location = new Point(16, 16),
                AutoSize = true
            };
            pnlLegend.Controls.Add(lblLegendTitle);

            pnlCountdown = new Panel
            {
                Size = new Size(340, 95),
                Location = new Point(24, 410),
                BackColor = ColorTranslator.FromHtml("#181c20")
            };

            lblCountdownTitle = new Label
            {
                Text = "THỜI GIAN GIỮ GHẾ CÒN LẠI",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                Location = new Point(16, 14),
                AutoSize = true
            };

            lblCountdown = new Label
            {
                Text = "03:00",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(16, 40),
                AutoSize = true
            };

            pnlCountdown.Controls.Add(lblCountdownTitle);
            pnlCountdown.Controls.Add(lblCountdown);

            pnlLeftInfo.Controls.AddRange(new Control[] { lblMovieTitle, lblShowtimeInfo, pnlLegend, pnlCountdown });

            pnlScreenArea = new Panel
            {
                Size = new Size(1468, 876),
                Location = new Point(428, 96),
                BackColor = ColorTranslator.FromHtml("#101418")
            };

            lblScreen = new Label
            {
                Text = "M À N   H Ì N H   C H I Ế U   ( S C R E E N )",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#38bdf8"),
                BackColor = ColorTranslator.FromHtml("#1e293b"),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(1000, 40),
                Location = new Point(234, 20)
            };
            pnlScreenArea.Controls.Add(lblScreen);

            pnlBottomBar = new Panel
            {
                Size = new Size(1920, 84),
                Location = new Point(0, 988),
                BackColor = ColorTranslator.FromHtml("#0b0f12")
            };

            lblSelectedSeats = new Label
            {
                Text = "GHẾ ĐÃ CHỌN: (Chưa chọn ghế)",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(428, 30),
                AutoSize = true
            };

            lblTotalPrice = new Label
            {
                Text = "TỔNG TIỀN: 0 đ",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(1100, 26),
                AutoSize = true
            };

            btnContinuePayment = new Button
            {
                Text = "CHỌN BẮP NƯỚC →",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#101418"),
                BackColor = ColorTranslator.FromHtml("#282C31"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 52),
                Location = new Point(1660, 16),
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnContinuePayment.FlatAppearance.BorderSize = 0;

            pnlBottomBar.Controls.AddRange(new Control[] { lblSelectedSeats, lblTotalPrice, btnContinuePayment });

            this.Controls.AddRange(new Control[] { pnlHeader, pnlLeftInfo, pnlScreenArea, pnlBottomBar });
            this.ResumeLayout(false);
        }

        #endregion
    }